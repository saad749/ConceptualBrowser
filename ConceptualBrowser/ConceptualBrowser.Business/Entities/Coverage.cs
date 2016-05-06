﻿using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConceptualBrowser.Business.Common.Helpers;

namespace ConceptualBrowser.Business.Entities
{
    public class Coverage
    {
        public int MaxRank { get; set; } = 200; //C# 6.0 allows property initializers// WHY 200?????
        public List<Sentence> Sentences { get; set; }
        public int TotalSentences { get; set; }
        public BinaryRelation BinaryRelation { get; set; }
        public List<int[]> Pairs { get; set; } = new List<int[]>();
        public int CurrentConcept { get; set; } = -1;
        public List<OptimalConcept> OptimalConcepts { get; set; } = new List<OptimalConcept>();
        public ConceptTree HeapConcepts { get; set; } = new ConceptTree(1000);

        public IStemmer Stemmer { get; set; }
        public IEmptyWords EmptyWordsRoot { get; set; }


        public Coverage(IStemmer stemmer, IEmptyWords emptywords)
        {
            Stemmer = stemmer;
            EmptyWordsRoot = emptywords;
        }

        public void CreateBinaryRelation(List<string> sentenceStringList, List<Sentence> sentences)
        {
            int tempTotalWords = 0;

            Sentences = sentences;
            TotalSentences = sentences.Count;
            BinaryRelation = new BinaryRelation(TotalSentences, Stemmer);
            ITextAnalyzer textAnalyzer = new TextAnalyzer(Stemmer, EmptyWordsRoot);
            for (int i = 0; i < sentenceStringList.Count; i++)
            {
                List<string> wordsList = textAnalyzer.Tokenizer(sentenceStringList[i]);
                tempTotalWords += wordsList.Count;
                BinaryRelation.AppendToBinaryRelation(wordsList, sentences[i]);
            }

            Console.WriteLine("Total WOrds: " + tempTotalWords);

            this.KeywordsRank();
            this.AddHighestRankKeywords();

            BinaryRelation.KeywordsSentencesSum = BinaryRelation.Keywords.SelectMany(s => s.Sentences).Count();
            Console.WriteLine("KeywordsSentencesSum: " + BinaryRelation.KeywordsSentencesSum);
        }

        public void KeywordsRank()
        {
            List<KeywordNode> keywords = new List<KeywordNode>();
            //keywords = BinaryRelation.Keywords.Values.ToList();
            keywords = BinaryRelation.Keywords.ToList();

            for (int i = 0; i < keywords.Count; i++)
            {
                KeywordNode key = keywords[i];
                key.KeywordRank = (1 / key.KeywordRank); //This changes the keyword rank in the BinaryRelation Keywords List.
            }   
        }

        private void AddHighestRankKeywords()
        {
            List<string> usedKeywords = new List<string>();
            List<Sentence> namedSentences = new List<Sentence>();

            double max = MaxRank;
            int KeywordNo = -1;
            String keywordString = null;
            //List<KeywordNode> keywords = BinaryRelation.Keywords.Values.ToList();
            List<KeywordNode> keywords = BinaryRelation.Keywords.ToList();

            for (int t = 0; t < keywords.Count; t++)
            {
                List<Sentence> sentences = keywords[t].Sentences;

                for (int i = 0; i < sentences.Count; i++)
                {
                    Sentence sentence = sentences[i];
                    if (!InNamedList(namedSentences, sentence))
                    {
                        for (int j = 0; j < keywords.Count; j++)
                        {
                            KeywordNode keywordNode = BinaryRelation.Keywords[j];
                            if (keywordNode.ContainsSentenceIndex(sentence.SentenceIndex))
                            {
                                if (!usedKeywords.Contains(keywordNode.Keyword))
                                    if (keywordNode.KeywordRank < max)
                                    {
                                        max = keywordNode.KeywordRank;
                                        KeywordNo = keywordNode.KeywordIndex;
                                        keywordString = keywordNode.Keyword;
                                    }
                            }
                        }
                        if (keywordString == null)
                        {
                            usedKeywords.Clear();
                            max = MaxRank;
                            KeywordNo = -1;
                            keywordString = null;
                            i--;
                        }
                        else
                        {
                            sentence.KeywordNumber = KeywordNo;
                            String key = (BinaryRelation.GetRootNode(keywordString)).getMaxLengthWord();
                            sentence.KeywordString = key;
                            max = MaxRank;
                            usedKeywords.Add(keywordString);
                            namedSentences.Add(sentence);
                        }
                    }
                }
            }
        }

        public bool InNamedList(List<Sentence> sentences, Sentence sentence)
        {
            for (int i = 0; i < sentences.Count; i++)
            {
                if (sentences[i].SentenceIndex == sentence.SentenceIndex) //This was a bug from Last Code Conversion //node[i] was node Only
                {
                    sentence.KeywordString = sentences[i].KeywordString;
                    sentence.KeywordNumber = sentences[i].KeywordNumber;
                    return true;
                }
            }

            return false;
        }

        public List<OptimalConcept> ExtractAll()
        {
            //Parallel.For(0, 4, e => {

                int[] next = this.NextNonCovered(BinaryRelation);
                ExtractConcepts(next);
            //});

            //ExtractConcepts(next);
            

            OptimalConcepts.OrderByDescending(o => o.Gain);

            //this.AddToHeap();

            for (int i = 0; i < OptimalConcepts.Count; i++)
                OptimalConcepts[i].SetConceptName(this.BinaryRelation);


            OptimalConcepts = OptimalConcepts.GroupBy(o => o.ConceptName).Select(g => g.First()).ToList();

            return OptimalConcepts;
        }

        private void ExtractConcepts(int[] next)
        {
            //Console.WriteLine();
            while (next != null)
            {
                //Console.WriteLine("Next: " + next[0] + " | " + next [1]);
                this.ExtractOptimalConcept(this.BinaryRelation, next[0], next[1]);
                next = this.NextNonCovered(this.BinaryRelation);
            }
        }

        private void AddToHeap()
        {
            for (int i = 0; i < OptimalConcepts.Count; i++)
                AddToHeapOfConcepts(i, (OptimalConcepts[i].ConceptNumber));
        }


        //get the next non covered tuple in the BR
        //It is a simple method, that goes through all the keywords in the binary relation and chooses that one that is yet not covered.
        //We can Apply Randomization here!!
        public int[] NextNonCovered(BinaryRelation binaryRelation)
        {
            //List<KeywordNode> keywords = binaryRelation.Keywords.Values.ToList();
            List<KeywordNode> keywords = binaryRelation.Keywords.ToList();
            //Console.WriteLine("KeywordsCount: " + keywords.Count);

            //foreach (KeywordNode keyword in keywords)
            //{
            //    //LogHelper.PrintKeyword(keyword, "NextNonCovered - ");
            //    List<Sentence> sentences = keyword.Sentences;
            //    foreach (Sentence sentence in sentences)
            //    {
            //        //LogHelper.PrintSentence(sentence, "NextNonCovered - ");
            //        if (sentence.CoveredByConceptNumber < 0)
            //        {
            //            int[] indexes = { keyword.KeywordIndex, sentence.SentenceIndex };
            //            return indexes;
            //        }
            //    }
            //}
            int i = 0;
            //Console.WriteLine("Total Unique: " + BinaryRelation.TotalUniqueCovered + "  Total Sentences: " + TotalSentences);
            keywords = keywords.Where(k => k.Sentences.Any(s => s.CoveredByConceptNumber == -1)).ToList(); // This slows down but will surely complete the coverage
            while (BinaryRelation.TotalUniqueCovered < BinaryRelation.KeywordsSentencesSum) //(keywords.Count * 100)
            {
                Random random = new Random();

                int randomKeywordIndex = random.Next(0, keywords.Count);
                int randomSentenceIndex = random.Next(0, keywords[randomKeywordIndex].Sentences.Count);

                if (keywords[randomKeywordIndex].Sentences[randomSentenceIndex].CoveredByConceptNumber < 0)
                {
                    int[] indexes = { keywords[randomKeywordIndex].KeywordIndex,
                        keywords[randomKeywordIndex].Sentences[randomSentenceIndex].SentenceIndex };

                    return indexes;
                }

                i++;
            }

            return null;
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
            equivalentRectangle.GetInverse(Sentences);
            List<int[]> tuples = equivalentRectangle.ConvertToElementaryRelation(keywordIndex, sentenceIndex);
            ExtractOptimalConcepts(equivalentRectangle, tuples, keywordIndex, sentenceIndex);
        }

        // extract optimal concept that cover the tuple(k,u)
        public void ExtractOptimalConcepts(EquivalentRectangle equivalentRectangle, List<int[]> tuple, int keywordIndex, int sentenceIndex)
        {
            //LogHelper.PrintListArray(tuple, "ExtractOptimalConcepts -- List Array - k: " + k + " | u: " + u);
            bool conceptExtracted = false;
            EquivalentRectangle temp1 = new EquivalentRectangle();
            EquivalentRectangle temp2 = new EquivalentRectangle();
            temp1 = equivalentRectangle.Clone();
            temp2 = equivalentRectangle.Clone();

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
                for (int t = 0; t < tuple.Count; t++)
                {
                    int[] pair = tuple[t];
                    if (!InPairs(Pairs, pair[0], pair[1]))
                    {
                        temp1.ConvertToElementaryRelation(pair[0], pair[1]);
                        gain = temp1.CalculateEconomy(BinaryRelation);
                        if (gain > max)
                        {
                            max = gain;
                            highestEquivalentRectangle.Equate(temp1);
                            tempKeywordIndex = pair[0];
                            tempSentenceIndex = pair[1];
                        }
                        temp1 = temp2.Clone();
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
                    if (!highestEquivalentRectangle.IsRectangle()) 
                    {
                        temp1 = highestEquivalentRectangle.Clone();
                        temp2 = highestEquivalentRectangle.Clone();
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
                        AddToCoverage(highestEquivalentRectangle.ConvertToConcept(this.BinaryRelation, CurrentConcept, gain));
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

                if (pair == null)//This is a parallelization CHeck!
                    continue;

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

        // sort the list of concepts depende on the gain of each one
        private void Sort()
        {
            OptimalConcepts = OptimalConcepts.Where(o => o != null).ToList();//This is a parallelization CHeck!
            /*********************Bubble sort*********************/
            int index = -1;
            for (int j = 0; j < this.OptimalConcepts.Count; j++)
            {
                index = -1;
                OptimalConcept optimal = OptimalConcepts[j];
                for (int i = j + 1; i < OptimalConcepts.Count; i++)
                {
                    if (optimal.Gain < OptimalConcepts[i].Gain)
                    {
                        optimal = OptimalConcepts[i];
                        index = i;
                    }
                    else if ((optimal.Gain == OptimalConcepts[i].Gain) && (optimal.NodesTree.TotalNodes < OptimalConcepts[i].NodesTree.Height))
                    {
                        optimal = OptimalConcepts[i];
                        index = i;
                    }
                }
                if (index != -1)
                {
                    OptimalConcept optimal2 = OptimalConcepts[j];
                    this.OptimalConcepts[j] = optimal;
                    this.OptimalConcepts[index] =  optimal2;
                }
            }
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
