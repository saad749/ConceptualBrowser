using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConceptualBrowser.Business.Common.Helpers;
using ConceptualBrowser.Business.Common;

namespace ConceptualBrowser.Business.Entities
{
    public class BinaryRelation
    {
        public List<KeywordNode> Keywords { get; set; } //Changing Dictionary to List
        public int TotalResults { get; set; }
        public List<RootNode> Roots { get; set; } = new List<RootNode>();
        public List<string> PrimaryConceptsName { get; set; } = new List<string>();
        public List<Sentence> Sentences { get; set; }
        public int TotalSentences { get; set; }
        public int MaxRank { get; set; } = 2; //2 because the value can never be mroe than 1 //C# 6.0 allows property initializers// WHY 200?????
        public int KeywordsSentencesSum { get; set; }
        public int TotalUniqueCovered { get; set; }
        public TextAnalyzer TextAnalyzer { get; set; }

        // PERFORMANCE OPTIMIZATION: Fast lookup dictionaries for O(1) access instead of O(n) list searches
        private Dictionary<string, KeywordNode> _keywordLookup = new Dictionary<string, KeywordNode>();
        private Dictionary<string, RootNode> _rootLookup = new Dictionary<string, RootNode>();

        // PERFORMANCE OPTIMIZATION: Thread-safe collections for parallel processing
        private readonly object _keywordLookupLock = new object();
        private readonly object _rootLookupLock = new object();
        private readonly object _keywordsLock = new object();

        // PERFORMANCE OPTIMIZATION: Parallel processing configuration
        public bool EnableParallelProcessing { get; set; } = true;
        public int ParallelThreshold { get; set; } = 50; // Use parallel processing for 50+ sentences

        public BinaryRelation(string languageCode, string text)
        {
            Keywords = new List<KeywordNode>();
            // PERFORMANCE: Initialize fast lookup dictionaries
            _keywordLookup = new Dictionary<string, KeywordNode>();
            _rootLookup = new Dictionary<string, RootNode>();

            TextAnalyzer = new TextAnalyzer(languageCode);
            List<String> sentenceList = TextAnalyzer.GetSentences(TextAnalyzer.RemoveDiacritics(text));
            CreateBinaryRelation(sentenceList);
        }

        public void CreateBinaryRelation(List<string> sentenceStringList)
        {
            int tempTotalWords = 0;
            Sentences = new List<Sentence>();
            for (int i = 0; i < sentenceStringList.Count; i++)
            {
                int[] ranks = new int[] { i + 1, i + 1 };
                int[] totals = new int[] { (sentenceStringList.Count + 2) / 2, (sentenceStringList.Count + 2) / 2 };
                Rank rank = new Rank(2, ranks, totals);

                Sentences.Add(new Sentence(i, Constant.NotCovered, rank, sentenceStringList[i]));
            }

            TotalSentences = Sentences.Count;

            // PERFORMANCE OPTIMIZATION: Use parallel processing for large texts
            if (EnableParallelProcessing && sentenceStringList.Count >= ParallelThreshold)
            {
                tempTotalWords = ProcessSentencesParallel(sentenceStringList);
            }
            else
            {
                tempTotalWords = ProcessSentencesSequential(sentenceStringList);
            }

            Console.WriteLine("Total Words: " + tempTotalWords);

            this.KeywordsRank();
            this.AddHighestRankKeywords();

            KeywordsSentencesSum = Keywords.SelectMany(s => s.Sentences).Count();
            Console.WriteLine("KeywordsSentencesSum: " + KeywordsSentencesSum);
        }

        // PERFORMANCE OPTIMIZATION: Sequential sentence processing for smaller texts
        private int ProcessSentencesSequential(List<string> sentenceStringList)
        {
            int totalWords = 0;
            for (int i = 0; i < sentenceStringList.Count; i++)
            {
                List<string> wordsList = TextAnalyzer.Tokenizer(sentenceStringList[i]);
                totalWords += wordsList.Count;
                AppendToBinaryRelation(wordsList, Sentences[i]);
            }
            return totalWords;
        }

        // PERFORMANCE OPTIMIZATION: Parallel sentence processing for large texts
        private int ProcessSentencesParallel(List<string> sentenceStringList)
        {
            // Use ConcurrentDictionary for thread-safe processing
            var results = new ConcurrentBag<(int index, List<string> words)>();

            // Process sentences in parallel
            Parallel.For(0, sentenceStringList.Count, i =>
            {
                List<string> wordsList = TextAnalyzer.Tokenizer(sentenceStringList[i]);
                results.Add((i, wordsList));
            });

            // Aggregate results sequentially to maintain consistency
            int totalWords = 0;
            foreach (var (index, words) in results.OrderBy(r => r.index))
            {
                totalWords += words.Count;
                AppendToBinaryRelationThreadSafe(words, Sentences[index]);
            }

            return totalWords;
        }

        public void KeywordsRank()
        {
            List<KeywordNode> keywords = new List<KeywordNode>();
            //keywords = BinaryRelation.Keywords.Values.ToList();
            keywords = Keywords.ToList();

            for (int i = 0; i < keywords.Count; i++)
            {
                KeywordNode key = keywords[i];
                key.KeywordRank = (1 / key.KeywordRank); //This changes the keyword rank in the BinaryRelation Keywords List.
            }
        }

        private void AddHighestRankKeywords()
        {
            double max = MaxRank;
            int KeywordNo = -1;
            String keywordString = null;

            for (int i = 0; i < Sentences.Count; i++)
            {
                Sentence sentence = Sentences[i];
                for (int j = 0; j < sentence.KeywordNodes.Count; j++)
                {
                    KeywordNode keywordNode = sentence.KeywordNodes[j];

                    if (keywordNode.KeywordRank < max)
                    {
                        max = keywordNode.KeywordRank;
                        KeywordNo = keywordNode.KeywordIndex;
                        keywordString = keywordNode.Keyword;
                    }
                }
                if (keywordString == null)
                {
                    max = MaxRank;
                    KeywordNo = -1;
                    keywordString = null;
                    i--;
                }
                else
                {
                    sentence.KeywordNumber = KeywordNo;
                    String key = (GetRootNode(keywordString)).getMaxLengthWord();
                    sentence.KeywordString = key;
                    max = MaxRank;
                }
            }
        }






        public RootNode GetRootNode(String keyword)
        {
            return Roots.FirstOrDefault(r => r.Root.Equals(keyword, StringComparison.OrdinalIgnoreCase));
        }

        public void AppendToBinaryRelation(List<String> words, Sentence sentence)
        {
            sentence.KeywordNodes = new List<KeywordNode>();
            foreach (string word in words)
            {
                //These temporary Variables are SUPER VARIABLES. DONT EVEN THINK TO REMOVE THEM. THIS WILL SKIP A LOT OF
                //CONCEPTS. AND CAN TAKE FOR EVER TO UNDERSTAND!!!
                Sentence tempSentence = new Sentence(sentence.SentenceIndex, sentence.LastCoveredByConceptNumber, sentence.Rank, sentence.KeywordNodes, sentence.OriginalSentence); // Why to create a tempSentence? -s refers to this variable
                String tempWord = word; //Why again? Why create tempVariables?? -k refers to this variable


                String stem = TextAnalyzer.Stem(tempWord.ToLower());//-k
                RootNode root = new RootNode();
                
                //PERFORMANCE OPTIMIZATION: Use O(1) dictionary lookup instead of O(n) FirstOrDefault
                _keywordLookup.TryGetValue(stem, out KeywordNode keyword);

                if (keyword != null)
                {
                    //If the stram of the word already exists in the List of keywords with the Binary Relations Then 

                    //KeywordNode tempKeywordNode = Keywords.FirstOrDefault(v => v.Keyword == stem);
                    if (!keyword.Sentences.Any( n => n.SentenceIndex == tempSentence.SentenceIndex)) 
                    {
                        //If the word exists in the list of key words, then it checks if the word has any sentences that has a sentenceIndex
                        //that matches the sentence that word was found in (if not its probably repeated in the same sentence and ignored)... THEN
                        //Increase the Rank of the Keyword
                        //For some reason if the word doesnt exists in Roots List of Binary Relation, then Add it to Binary Relation .. Word or Stem??
                        //Add the sentence to the list of the sentences to the Keyword that has the stem of this word

                        keyword.KeywordRank++;
                        //PERFORMANCE OPTIMIZATION: Use O(1) dictionary lookup instead of O(n) FirstOrDefault
                        if (_rootLookup.TryGetValue(stem.ToLowerInvariant(), out root) && root != null)
                        {
                            if (!root.ExistsInOriginalWords(tempWord))//-k
                                root.OriginalWords.Add(tempWord);//-k
                        }

                        //tempSentence.KeywordNodes.Add(keyword);//Addition for Speed
                        sentence.KeywordNodes.Add(keyword);
                        keyword.Sentences.Add(tempSentence); //Referencing issue may be???//-s

                        // PERFORMANCE OPTIMIZATION: Also maintain index-based relationships for memory efficiency
                        sentence.KeywordIndexes.Add(keyword.KeywordIndex);
                        keyword.SentenceIndexes.Add(tempSentence.SentenceIndex);

                        
                        
                    }
                }
                else
                {
                    //If the stem of the word doesnt already exists in the Binary Relation List of Keywords Then
                    //It creates a Keyword Node and adds it to its list. 
                    //It also creates a Root node and add it to list
                    List<Sentence> sentences = new List<Sentence>();
                    sentences.Add(tempSentence);//-s
                    List<string> orginalWords = new List<string>();
                    orginalWords.Add(tempWord);//-k

                    root = new RootNode(stem, orginalWords);
                    Roots.Add(root);
                    //PERFORMANCE OPTIMIZATION: Add to dictionary for fast lookups
                    _rootLookup[stem.ToLowerInvariant()] = root;

                    KeywordNode temp = new KeywordNode(stem, Keywords.Count, 1, sentences);

                    //tempSentence.KeywordNodes.Add(temp);//Addition for Speed - By Reference!!
                    sentence.KeywordNodes.Add(temp);
                    Keywords.Add(temp);
                    //PERFORMANCE OPTIMIZATION: Add to dictionary for fast lookups
                    _keywordLookup[stem] = temp;

                    // PERFORMANCE OPTIMIZATION: Also maintain index-based relationships for memory efficiency
                    sentence.KeywordIndexes.Add(temp.KeywordIndex);
                    // Note: SentenceIndexes is already populated in KeywordNode constructor
                }
            }
        }

        // PERFORMANCE OPTIMIZATION: Thread-safe version of AppendToBinaryRelation for parallel processing
        public void AppendToBinaryRelationThreadSafe(List<String> words, Sentence sentence)
        {
            sentence.KeywordNodes = new List<KeywordNode>();
            foreach (string word in words)
            {
                Sentence tempSentence = new Sentence(sentence.SentenceIndex, sentence.LastCoveredByConceptNumber, sentence.Rank, sentence.KeywordNodes, sentence.OriginalSentence);
                String tempWord = word;

                String stem = TextAnalyzer.Stem(tempWord.ToLower());
                RootNode root = new RootNode();

                KeywordNode keyword = null;
                // Thread-safe keyword lookup
                lock (_keywordLookupLock)
                {
                    _keywordLookup.TryGetValue(stem, out keyword);
                }

                if (keyword != null)
                {
                    // Check if sentence already exists (thread-safe)
                    bool sentenceExists = false;
                    lock (keyword)
                    {
                        sentenceExists = keyword.Sentences.Any(n => n.SentenceIndex == tempSentence.SentenceIndex);
                    }

                    if (!sentenceExists)
                    {
                        // Update keyword rank (thread-safe)
                        lock (keyword)
                        {
                            keyword.KeywordRank++;
                        }

                        // Thread-safe root lookup and update
                        lock (_rootLookupLock)
                        {
                            if (_rootLookup.TryGetValue(stem.ToLowerInvariant(), out root) && root != null)
                            {
                                lock (root)
                                {
                                    if (!root.ExistsInOriginalWords(tempWord))
                                        root.OriginalWords.Add(tempWord);
                                }
                            }
                        }

                        // Thread-safe updates
                        sentence.KeywordNodes.Add(keyword);
                        lock (keyword)
                        {
                            keyword.Sentences.Add(tempSentence);
                            keyword.SentenceIndexes.Add(tempSentence.SentenceIndex);
                        }
                        sentence.KeywordIndexes.Add(keyword.KeywordIndex);
                    }
                }
                else
                {
                    // Create new keyword and root (thread-safe)
                    List<Sentence> sentences = new List<Sentence> { tempSentence };
                    List<string> originalWords = new List<string> { tempWord };

                    root = new RootNode(stem, originalWords);

                    KeywordNode temp;
                    lock (_keywordsLock)
                    {
                        temp = new KeywordNode(stem, Keywords.Count, 1, sentences);
                        Keywords.Add(temp);
                        Roots.Add(root);
                    }

                    // Update lookups (thread-safe)
                    lock (_rootLookupLock)
                    {
                        _rootLookup[stem.ToLowerInvariant()] = root;
                    }

                    lock (_keywordLookupLock)
                    {
                        _keywordLookup[stem] = temp;
                    }

                    sentence.KeywordNodes.Add(temp);
                    sentence.KeywordIndexes.Add(temp.KeywordIndex);
                }
            }
        }

        /// <summary>
        /// Returns Count of All Sentences in the Binary Relation. Is this right? Should it be Keywords.Count * Sentences.Count ?
        /// </summary>
        /// <returns></returns>
        public int GetTupleCount()
        {
            return Keywords.ToList().Sum(k => k.Sentences.Count);
        }

        public void MarkAsCovered(List<int[]> tuples, int current)
        {
            //LogHelper.PrintListArray(tuples, "MarkAsCovered -- List Array - ");
            for (int i = 0; i < tuples.Count; i++)
            {
                int[] pair = tuples[i];
                KeywordNode keyword = Keywords[pair[0]];
                //LogHelper.PrintKeyword(keyword, "Marked As Covered: ");
                if (keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).LastCoveredByConceptNumber == -1) //THis Part is important as it is used for Randomizations!
                {
                    TotalUniqueCovered++;
                    //Console.WriteLine("Unique Cover" + TotalUniqueCovered);
                }

                keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).LastCoveredByConceptNumber = current;
                keyword.Sentences.FirstOrDefault(n => n.SentenceIndex == pair[1]).CovertedbyConceptNumbers.Add(current); //Added to keep track of concept numbers
            }
        }

        public bool InPrimaryConceptsName(string name)
        {
            return PrimaryConceptsName.Any(p => p.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        //	 set all RootNodes status as false, not covered
        public void ResetRootNodes()
        {
            Roots.ForEach(r => r.Covered = false);
        }

    }
}
