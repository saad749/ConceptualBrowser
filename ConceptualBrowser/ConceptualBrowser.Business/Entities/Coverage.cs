using ConceptualBrowser.Business.Common.Stemmer;
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
        public int MaxRank { get; set; } = 2; //2 because the value can never be mroe than 1 //C# 6.0 allows property initializers// WHY 200?????
        public List<Sentence> Sentences { get; set; }
        public int TotalSentences { get; set; }
        public BinaryRelation BinaryRelation { get; set; }
        public List<int[]> Pairs { get; set; } = new List<int[]>();
        public int CurrentConcept { get; set; } = -1;
        public List<OptimalConcept> OptimalConcepts { get; set; } = new List<OptimalConcept>();
        public ConceptTree HeapConcepts { get; set; } //= new ConceptTree(1000);

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
                    String key = (BinaryRelation.GetRootNode(keywordString)).getMaxLengthWord();
                    sentence.KeywordString = key;
                    max = MaxRank;
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
            //int[] next = this.NextNonCovered(BinaryRelation);
            ExtractConcepts();

            //OptimalConcepts = OptimalConcepts.OrderByDescending(o => o.Gain).ToList();
            this.Sort(); // Changing the sort method can have consequences on correct output compared to master branch


            HeapConcepts = new ConceptTree(OptimalConcepts.Count);
            for (int i = 0; i < OptimalConcepts.Count; i++)
                AddToHeapOfConcepts(i, (OptimalConcepts[i].ConceptNumber));

            foreach (OptimalConcept optimalConcept in OptimalConcepts)
                optimalConcept.SetConceptName(this.BinaryRelation);

            return OptimalConcepts;
        }

        private void ExtractConcepts()
        {
                List<KeywordNode> keywords = BinaryRelation.Keywords.ToList();

                foreach (KeywordNode keyword in keywords)
                {
                    List<Sentence> sentences = keyword.Sentences;
                    foreach (Sentence sentence in sentences)
                    {
                        if (sentence.CoveredByConceptNumber < 0)
                        {
                            int[] indexes = { keyword.KeywordIndex, sentence.SentenceIndex };
                            if (indexes != null)
                            {
                                this.ExtractOptimalConcept(this.BinaryRelation, indexes[0], indexes[1]);
                            }
                        }
                    }
                }

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
