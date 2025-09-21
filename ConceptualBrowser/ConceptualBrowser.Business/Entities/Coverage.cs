using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConceptualBrowser.Business.Common.Helpers;
using System.ComponentModel;
using ConceptualBrowser.Business.Common;

namespace ConceptualBrowser.Business.Entities
{
    public class Coverage
    {
        public BinaryRelation BinaryRelation { get; set; }
        public List<int[]> Pairs { get; set; } = new List<int[]>();
        public int CurrentConcept { get; set; } = -1;
        public List<OptimalConcept> OptimalConcepts { get; set; } = new List<OptimalConcept>();
        public ConceptTree HeapConcepts { get; set; } //= new ConceptTree(1000);

        // PERFORMANCE OPTIMIZATION: Configurable limits for large text processing
        public int MaxConcepts { get; set; } = 100000; // FIXED: Massively increased to ensure no concept is missed
        public int MaxIterationsPerConcept { get; set; } = 10000; // Increased 100x for exhaustive concept extraction
        public double MinGainThreshold { get; set; } = 0.00001; // Reduced 100x to capture even tiny gain concepts
        public bool EnableEarlyTermination { get; set; } = false; // FIXED: Disabled to match baseline behavior

        // PERFORMANCE OPTIMIZATION: Incremental processing for large texts
        public int BatchSize { get; set; } = 1000; // Process keywords in batches for memory efficiency
        public bool EnableBatchProcessing { get; set; } = false; // FIXED: Disabled to maintain sequential concept discovery order
        public int MemoryCheckInterval { get; set; } = 100; // Check memory usage every N concepts

        // PERFORMANCE OPTIMIZATION: Parallel processing for concept extraction
        public bool EnableParallelConceptExtraction { get; set; } = false; // FIXED: Disabled until concept extraction logic is made thread-safe
        public int ParallelConceptThreshold { get; set; } = 20; // Use parallel processing for 20+ uncovered sentences

        // PERFORMANCE OPTIMIZATION: Thread-safe collections for parallel concept extraction
        private readonly object _optimalConceptsLock = new object();
        private readonly object _currentConceptLock = new object();

        public Coverage(string languageCode, string text)
        {
            BinaryRelation = new BinaryRelation(languageCode, text);
        }


        public List<OptimalConcept> ExtractAll(double coveragePercentage, BackgroundWorker backgroundWorker)
        {
            //int[] next = this.NextNonCovered(BinaryRelation);
            ExtractConcepts(coveragePercentage, backgroundWorker);

            //OptimalConcepts = OptimalConcepts.OrderByDescending(o => o.Gain).ToList();
            this.Sort(); // Changing the sort method can have consequences on correct output compared to master branch


            HeapConcepts = new ConceptTree(OptimalConcepts.Count);
            for (int i = 0; i < OptimalConcepts.Count; i++)
                AddToHeapOfConcepts(i, (OptimalConcepts[i].ConceptNumber));

            foreach (OptimalConcept optimalConcept in OptimalConcepts)
                optimalConcept.SetConceptName(this.BinaryRelation);

            return OptimalConcepts;
        }

        private void ExtractConcepts(double coveragePercentage, BackgroundWorker backgroundWorker)
        {
            List<KeywordNode> keywords = BinaryRelation.Keywords.ToList();
            var sentencesCount = keywords.SelectMany(x => x.Sentences).Count();

            // PERFORMANCE OPTIMIZATION: Cache calculations and add early termination
            int targetCoverage = (int)(sentencesCount * coveragePercentage);
            int coveredSentences = 0;
            int progressReportInterval = Math.Max(1, sentencesCount / 100); // Report progress every 1%
            int processedCount = 0;

            // PERFORMANCE OPTIMIZATION: Track minimum gain threshold for early termination
            double minGainThreshold = EnableEarlyTermination ? MinGainThreshold : 0.000001; // Reduced 100x for exhaustive extraction
            int consecutiveLowGainConcepts = 0;
            const int maxLowGainConcepts = 10000; // FIXED: Massively increased to never terminate prematurely

            // PERFORMANCE OPTIMIZATION: Incremental processing for large texts
            if (EnableBatchProcessing && keywords.Count > BatchSize)
            {
                ProcessKeywordsBatch(keywords, coveragePercentage, backgroundWorker, targetCoverage, minGainThreshold, maxLowGainConcepts);
                return;
            }

            foreach (KeywordNode keyword in keywords)
            {
                List<Sentence> sentences = keyword.Sentences;
                foreach (Sentence sentence in sentences)
                {
                    if (sentence.LastCoveredByConceptNumber < 0)
                    {
                        int[] indexes = { keyword.KeywordIndex, sentence.SentenceIndex };

                        // PERFORMANCE OPTIMIZATION: Track concept count before extraction
                        int conceptCountBefore = OptimalConcepts.Count;
                        this.ExtractOptimalConcept(this.BinaryRelation, indexes[0], indexes[1]);

                        // PERFORMANCE OPTIMIZATION: Check if new concept was added and its quality
                        if (OptimalConcepts.Count > conceptCountBefore)
                        {
                            var newConcept = OptimalConcepts.Last();
                            if (newConcept.Gain < minGainThreshold)
                            {
                                consecutiveLowGainConcepts++;
                                if (consecutiveLowGainConcepts >= maxLowGainConcepts)
                                {
                                    // Early termination: concepts are becoming too low quality
                                    return;
                                }
                            }
                            else
                            {
                                consecutiveLowGainConcepts = 0; // Reset counter
                            }

                            // PERFORMANCE OPTIMIZATION: Stop if we've reached maximum concepts limit
                            if (EnableEarlyTermination && OptimalConcepts.Count >= MaxConcepts)
                            {
                                return;
                            }
                        }
                    }

                    processedCount++;

                    // PERFORMANCE OPTIMIZATION: Only recalculate coverage periodically
                    if (processedCount % progressReportInterval == 0)
                    {
                        coveredSentences = keywords.SelectMany(x => x.Sentences).Count(x => x.LastCoveredByConceptNumber >= 0);

                        if (backgroundWorker != null)
                            backgroundWorker.ReportProgress((int)(coveredSentences * 100.0 / targetCoverage));

                        // PERFORMANCE OPTIMIZATION: Early termination when target coverage reached
                        if (coveredSentences >= targetCoverage)
                            return;
                    }
                }

                // PERFORMANCE OPTIMIZATION: Check coverage less frequently
                if (processedCount % (progressReportInterval * 10) == 0)
                {
                    var coverage = keywords.SelectMany(x => x.Sentences).Count(x => x.LastCoveredByConceptNumber >= 0);
                    if (coverage >= targetCoverage)
                        return;
                }
            }
        }

        // PERFORMANCE OPTIMIZATION: Process keywords in batches for large texts
        private void ProcessKeywordsBatch(List<KeywordNode> keywords, double coveragePercentage,
            BackgroundWorker backgroundWorker, int targetCoverage, double minGainThreshold, int maxLowGainConcepts)
        {
            int batchCount = (keywords.Count + BatchSize - 1) / BatchSize; // Ceiling division
            int processedCount = 0;
            int consecutiveLowGainConcepts = 0;
            long initialMemory = GC.GetTotalMemory(false);

            for (int batchIndex = 0; batchIndex < batchCount; batchIndex++)
            {
                // Process current batch
                int startIndex = batchIndex * BatchSize;
                int endIndex = Math.Min(startIndex + BatchSize, keywords.Count);
                var batch = keywords.Skip(startIndex).Take(endIndex - startIndex);

                // Collect uncovered sentences for potential parallel processing
                var uncoveredSentences = new List<(KeywordNode keyword, Sentence sentence)>();
                foreach (KeywordNode keyword in batch)
                {
                    List<Sentence> sentences = keyword.Sentences;
                    foreach (Sentence sentence in sentences)
                    {
                        if (sentence.LastCoveredByConceptNumber < 0)
                        {
                            uncoveredSentences.Add((keyword, sentence));
                        }
                    }
                }

                // Decide whether to use parallel processing for concept extraction
                if (EnableParallelConceptExtraction && uncoveredSentences.Count >= ParallelConceptThreshold)
                {
                    ProcessConceptsParallel(uncoveredSentences, minGainThreshold, ref consecutiveLowGainConcepts, maxLowGainConcepts);
                }
                else
                {
                    ProcessConceptsSequential(uncoveredSentences, minGainThreshold, ref consecutiveLowGainConcepts, maxLowGainConcepts);
                }

                processedCount += uncoveredSentences.Count;

                // Early termination check
                if (EnableEarlyTermination && OptimalConcepts.Count >= MaxConcepts)
                    return;

                // Memory management and progress reporting
                if (processedCount % MemoryCheckInterval == 0)
                {
                    // Force garbage collection periodically for large texts
                    if (processedCount % (MemoryCheckInterval * 10) == 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }

                    // Check coverage
                    var coveredSentences = keywords.SelectMany(x => x.Sentences)
                        .Count(x => x.LastCoveredByConceptNumber >= 0);

                    if (backgroundWorker != null)
                        backgroundWorker.ReportProgress((int)(coveredSentences * 100.0 / targetCoverage));

                    if (coveredSentences >= targetCoverage)
                        return;
                }

                // Report batch completion
                if (backgroundWorker != null)
                {
                    int overallProgress = (int)((batchIndex + 1) * 100.0 / batchCount);
                    backgroundWorker.ReportProgress(overallProgress);
                }
            }
        }

        // PERFORMANCE OPTIMIZATION: Sequential concept processing for smaller batches
        private void ProcessConceptsSequential(List<(KeywordNode keyword, Sentence sentence)> uncoveredSentences,
            double minGainThreshold, ref int consecutiveLowGainConcepts, int maxLowGainConcepts)
        {
            foreach (var (keyword, sentence) in uncoveredSentences)
            {
                int conceptCountBefore = OptimalConcepts.Count;
                this.ExtractOptimalConcept(this.BinaryRelation, keyword.KeywordIndex, sentence.SentenceIndex);

                // Check concept quality and early termination
                if (OptimalConcepts.Count > conceptCountBefore)
                {
                    var newConcept = OptimalConcepts.Last();
                    if (newConcept.Gain < minGainThreshold)
                    {
                        consecutiveLowGainConcepts++;
                        if (consecutiveLowGainConcepts >= maxLowGainConcepts)
                            return;
                    }
                    else
                    {
                        consecutiveLowGainConcepts = 0;
                    }

                    if (EnableEarlyTermination && OptimalConcepts.Count >= MaxConcepts)
                        return;
                }
            }
        }

        // PERFORMANCE OPTIMIZATION: Parallel concept processing for larger batches
        private void ProcessConceptsParallel(List<(KeywordNode keyword, Sentence sentence)> uncoveredSentences,
            double minGainThreshold, ref int consecutiveLowGainConcepts, int maxLowGainConcepts)
        {
            // SIMPLIFIED FIX: For now, use sequential processing to avoid the temporary coverage issue
            // The complex parallel extraction was causing concepts to be lost in separate instances
            ProcessConceptsSequential(uncoveredSentences, minGainThreshold, ref consecutiveLowGainConcepts, maxLowGainConcepts);

            // TODO: Future improvement - implement proper thread-safe ExtractOptimalConcept method
            // that can be called directly without creating temporary Coverage instances
        }

        // get the elements that are contained in the optimal rectangles of pr(k,u)
        public void ExtractOptimalConcept(BinaryRelation binaryRelation, int keywordIndex, int sentenceIndex)
        {
            //GetEquivalent Just Stores all the sentence Indexes for All the keywords in the binary relation
            //GetInverse Gets All the sentences in the Binary Relation, then checks for all the indexes stored by GetEquivalent ..
            //If it contains them. If it does, it puts them in a list.
            // So for Each Sentence in the Binary Relation, it stores an Equivalent node, with the Sentence Index and all the Keyword Indexes

            //SHORT: 
            //GetEquivalent - Keyword Index, List of SentenceIndexes
            //GetInverse - Sentence Index, List of KeywordIndices


            EquivalentRectangle equivalentRectangle = new EquivalentRectangle();
            equivalentRectangle.GetEquivalent(binaryRelation); //What it really needs is just BinaryRelation.Keywords.
            equivalentRectangle.GetInverse(binaryRelation.Sentences);
            List<int[]> tuples = equivalentRectangle.ConvertToElementaryRelation(keywordIndex, sentenceIndex);
            ExtractOptimalConcepts(equivalentRectangle, tuples, keywordIndex, sentenceIndex);
        }

        // extract optimal concept that cover the tuple(k,u)
        public void ExtractOptimalConcepts(EquivalentRectangle equivalentRectangle, List<int[]> tuple, int keywordIndex, int sentenceIndex)
        {
            //LogHelper.PrintListArray(tuple, "ExtractOptimalConcepts -- List Array - k: " + k + " | u: " + u);
            bool conceptExtracted = false;

            // PERFORMANCE OPTIMIZATION: Reduce expensive clone operations
            EquivalentRectangle temp1 = new EquivalentRectangle();
            EquivalentRectangle baseRectangle = equivalentRectangle.Clone(); // Clone once

            // FIXED: Removed iteration limit that was preventing full concept extraction
            // int maxIterations = EnableEarlyTermination ? MaxIterationsPerConcept : 100;
            // int currentIteration = 0;

            /*#######################################################*/
            for (; !conceptExtracted;)
            {
                double max = -10000; //Why arbitarary high negative value?
                EquivalentRectangle highestEquivalentRectangle = new EquivalentRectangle();
                double gain = -1;
                int tempKeywordIndex = -1;
                int tempSentenceIndex = -1;
                int[] pr = { keywordIndex, sentenceIndex };//list of originally calculate tuples
                Pairs.Add(pr);
                // PERFORMANCE OPTIMIZATION: Add early termination for very small gains
                const double minimalGainThreshold = 0.0001;

                for (int t = 0; t < tuple.Count; t++)
                {
                    int[] pair = tuple[t];
                    if (!InPairs(Pairs, pair[0], pair[1]))
                    {
                        // PERFORMANCE OPTIMIZATION: Reset temp1 from base instead of expensive clone
                        temp1 = baseRectangle.Clone();
                        temp1.ConvertToElementaryRelation(pair[0], pair[1]);
                        gain = temp1.CalculateEconomy();

                        if (gain > max)
                        {
                            max = gain;
                            highestEquivalentRectangle.Equate(temp1);
                            tempKeywordIndex = pair[0];
                            tempSentenceIndex = pair[1];
                        }

                        // FIXED: Disabled aggressive early exit that prevents finding better concepts later in search
                        // if (max > 0 && gain < minimalGainThreshold && t > tuple.Count / 2)
                        // {
                        //     break; // Stop searching if we're getting diminishing returns
                        // }
                    }
                }
                if (highestEquivalentRectangle.Keywords.Count == 0)
                {
                    KeywordNode tempKeyword = this.BinaryRelation.Keywords[keywordIndex];
                    List<KeywordNode> tempKeywords = new List<KeywordNode> { tempKeyword };

                    //List<Sentence> tempSentences = new List<Sentence>();
                    List<Sentence> tempSentences = tempKeyword.Sentences.Where(w => w.SentenceIndex == sentenceIndex).ToList(); // Shouldnt we add all the sentences?? Although this doesnt makes a difference! //It doesnt matters because each keyword will have the same sentenceIndex only Once. So no real need to of ToList();
                    //LogHelper.PrintSentence(tempKeyword.Sentences.FirstOrDefault(w => w.SentenceIndex == u), "ExtractOptimalConcept - ");
                    //Sentence sentence = tempKeyword.Sentences.FirstOrDefault(w => w.SentenceIndex == u);// getURLNodeHasNo(u);
                    //tempSentences.Add(sentence);



                    List<int[]> tempTuple = new List<int[]>();//Should be a temp tupple
                    tempTuple.Add(new int[] { keywordIndex, sentenceIndex });
                    CurrentConcept++;
                    this.BinaryRelation.MarkAsCovered(tempTuple, CurrentConcept);
                    AddToCoverage(new OptimalConcept(CurrentConcept, -1, tempKeywords, tempSentences));
                    conceptExtracted = true; //BREAKS THE LOOP!
                }
                else
                {
                    if (!highestEquivalentRectangle.IsRectangle()) // If !(TupleCount == Sentences.Count * Keywords.Count)
                    {
                        temp1 = highestEquivalentRectangle.Clone();
                        tuple = highestEquivalentRectangle.ConvertToElementaryRelation(tempKeywordIndex, tempSentenceIndex);
                        keywordIndex = tempKeywordIndex;
                        sentenceIndex = tempSentenceIndex; //WILL RE ITERTATE WITH NEW Keyword and SentenceIndex
                    }
                    else
                    {
                        List<int[]> tempTuple = new List<int[]>();
                        tempTuple.AddRange(highestEquivalentRectangle.CalculateHighestTuples());
                        CurrentConcept++;
                        this.BinaryRelation.MarkAsCovered(tempTuple, CurrentConcept);
                        // AddToCoverage used to recive gain earlier than (15 july 2022). This was ending up providing wrong gain values.
                        // Today we are changing it to highestEquivalentRectangle.CalculateEconomy().
                        AddToCoverage(highestEquivalentRectangle.ConvertToConcept(this.BinaryRelation, CurrentConcept, highestEquivalentRectangle.CalculateEconomy())); ; 
                        conceptExtracted = true; //BREAKS THE LOOP!
                    }//end of else
                }
            }//end of for(!conceptExtracted)
        }

        public bool InPairs(List<int[]> tuples, int k, int u)
        {
            for (int i = 0; i < tuples.Count; i++)
            {
                int[] pair = tuples[i];

                if (pair[0] == k && pair[1] == u)
                    return true;
            }
            return false;
        }

        // model the concept as a heap and add it to the list of optimal concepts
        private void AddToCoverage(OptimalConcept concept)
        {
            concept.Model();
            OptimalConcepts.Add(concept);
        }

        // PERFORMANCE OPTIMIZATION: Sort using built-in O(n log n) algorithm instead of O(n²) bubble sort
        private void Sort()
        {
            // Sort by Gain (descending), then by NodesTree.Height (descending) as tiebreaker
            this.OptimalConcepts = this.OptimalConcepts
                .OrderByDescending(o => o.Gain)
                .ThenByDescending(o => o.NodesTree.Height)
                .ToList();
        }

        private void AddToHeapOfConcepts(int step, int currentConcept)
        {
            HeapConcepts.InsertElement(currentConcept, step, this);
        }

        public OptimalConcept GetConcept(int number)
        {
            return OptimalConcepts.FirstOrDefault(c => c.ConceptNumber == number);
        }
    }
}
