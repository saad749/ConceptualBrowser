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
        public int MaxRank { get; set; } = 200; //C# 6.0 allows property initializers
        public List<Node> Nodes { get; set; }
        public int TotalResults { get; set; }
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

        public void CreateBinaryRelation(List<string> sentences, List<Node> nodes, bool rankedDocs)
        {
            Nodes = nodes;
            TotalResults = nodes.Count;
            BinaryRelation = new BinaryRelation(TotalResults, MaxRank);
            ITextAnalyzer textAnalyzer = new TextAnalyzer(Stemmer, EmptyWordsRoot);
            for (int i = 0; i < sentences.Count; i++)
            {
                List<string> keyWords = textAnalyzer.Tokenizer(sentences[i]);

                BinaryRelation.AppendToBinaryRelation(keyWords, nodes[i], rankedDocs);
            }



            this.KeywordsRank(rankedDocs);
            this.AddHighestRankKeywords();
        }

        public void KeywordsRank(bool rankedDocs)
        {
            if (rankedDocs)
            {
                List<KeywordNode> keywords = new List<KeywordNode>();
                keywords = BinaryRelation.Keywords.Values.ToList();

                for (int i = 0; i < keywords.Count; i++)
                {
                    double max = MaxRank;

                    KeywordNode key = keywords[i];
                    List<Node> nodes = key.Nodes;

                    for (int j = 0; j < nodes.Count; j++)
                    {
                        Node node = nodes[j];
                        if (node.Rank.CalRank < max)
                            max = node.Rank.CalRank;
                    }
                    keywords[i].KeywordRank = max;
                }

            }
            else
            {
                List<KeywordNode> keywords = new List<KeywordNode>();
                keywords = BinaryRelation.Keywords.Values.ToList();

                for (int i = 0; i < keywords.Count; i++)
                {
                    KeywordNode key = keywords[i];
                    key.KeywordRank = (1 / key.KeywordRank); //Divide By Zerooo????
                }
            }
        }

        private void AddHighestRankKeywords()
        {
            List<string> usedKeywords = new List<string>();
            List<Node> namedNodes = new List<Node>();

            double max = MaxRank;
            int KeywordNo = -1;
            String keywordString = null;
            List<KeywordNode> keywords = BinaryRelation.Keywords.Values.ToList();

            for (int t = 0; t < keywords.Count; t++)
            {
                List<Node> nodes = keywords[t].Nodes;

                for (int i = 0; i < nodes.Count; i++)
                {
                    Node node = nodes[i];
                    if (!InNamedList(namedNodes, node))
                    {
                        for (int j = 0; j < keywords.Count; j++)
                        {
                            KeywordNode keywordNode = BinaryRelation.Keywords[j];
                            if (keywordNode.ContainsWord(node.Word))
                            {
                                if (!usedKeywords.Contains(keywordNode.Keyword))
                                    if (keywordNode.KeywordRank < max)
                                    {
                                        max = keywordNode.KeywordRank;
                                        KeywordNo = keywordNode.Number;
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
                            node.KeywordNumber = KeywordNo;
                            String key = (BinaryRelation.GetRootNode(keywordString)).getMaxLengthWord();
                            node.KeywordString = key;
                            max = MaxRank;
                            usedKeywords.Add(keywordString);
                            namedNodes.Add(node);
                        }
                    }
                }
            }
        }

        public bool InNamedList(List<Node> nodes, Node node)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if (node.Word.Equals(node.Word, StringComparison.OrdinalIgnoreCase))
                {
                    node.KeywordString = nodes[i].KeywordString;
                    node.KeywordNumber = nodes[i].KeywordNumber;
                    return true;
                }
            }

            return false;
        }

        public List<OptimalConcept> ExtractAll()
        {
            int[] next = this.NextNonCovered(BinaryRelation);
            //Console.WriteLine();
            while (next != null)
            {
                //Console.WriteLine("Next: " + next[0] + " | " + next[1]);
                this.ExtractOptimalConcept(this.BinaryRelation, next[0], next[1]);
                next = this.NextNonCovered(this.BinaryRelation);
            }

            this.Sort();

            HeapConcepts = new ConceptTree(OptimalConcepts.Count);
            for (int i = 0; i < OptimalConcepts.Count; i++)
                AddToHeapOfConcepts(i, (OptimalConcepts[i].ConceptNumber));


            for (int i = 0; i < OptimalConcepts.Count; i++)
                OptimalConcepts[i].SetConceptName(this.BinaryRelation);
            return OptimalConcepts;
        }

        //get the next non covered tuple in the BR
        public int[] NextNonCovered(BinaryRelation binaryRelation)
        {
            List<KeywordNode> keywords = binaryRelation.Keywords.Values.ToList();
            //Console.WriteLine("KeywordsCount: " + keywords.Count);

            for (int i = 0; i < keywords.Count; i++)
            {
                KeywordNode keyword = keywords[i];
                //LogHelper.PrintKeyword(keyword, "NextNonCovered - ");

                List<Node> nodes = keyword.Nodes;
                for (int j = 0; j < nodes.Count; j++)
                {
                    Node node = nodes[j];
                    //LogHelper.PrintNode(node, "NextNonCovered - ");
                    if (node.CoveredBy < 0)
                    {
                        int[] indexes = { keyword.Number, node.Number };
                        return indexes;
                    }
                }
                //Console.WriteLine();
            }
            return null;
        }

        // get the elements that are contained in the optimal rectangles of pr(k,u)
        public void ExtractOptimalConcept(BinaryRelation R, int k, int u)
        {

            EquivalentRectangle equivalentRectangle = new EquivalentRectangle();
            equivalentRectangle.GetEquivalent(R);
            equivalentRectangle.GetInverse(Nodes);
            List<int[]> tuples = equivalentRectangle.convertR_PR(k, u);
            ExtractOptimalConcepts(equivalentRectangle, tuples, k, u);
        }

        // extract optimal concept that cover the tuple(k,u)
        public void ExtractOptimalConcepts(EquivalentRectangle equivalentRectangle, List<int[]> tuple, int k, int u)
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
                double max = -10000;
                EquivalentRectangle heighest = new EquivalentRectangle();
                double e = -1;
                int kk = -1;
                int uu = -1;
                int[] pr = { k, u };//list of originally calculate tuples
                Pairs.Add(pr);
                for (int t = 0; t < tuple.Count; t++)
                {
                    int[] pair = tuple[t];
                    if (!InPairs(Pairs, pair[0], pair[1]))
                    {
                        temp1.convertR_PR(pair[0], pair[1]);
                        e = temp1.CalculateEconomy(BinaryRelation);
                        if (e > max)
                        {
                            max = e;
                            heighest.equate(temp1);
                            kk = pair[0];
                            uu = pair[1];
                        }
                        temp1 = temp2.Clone();
                    }
                }
                if (heighest.Keywords.Count == 0)
                {
                    KeywordNode tempKeyword = this.BinaryRelation.Keywords[k];
                    List<KeywordNode> temp_keywords = new List<KeywordNode>();
                    temp_keywords.Add(tempKeyword);

                    List<Node> temp_Nodes = new List<Node>();
                    //Console.WriteLine();
                    //LogHelper.PrintNode(tempKeyword.Nodes.FirstOrDefault(w => w.Number == u), "ExtractOptimalConcept - ");
                    //Console.WriteLine("U: " + u);
                    //Console.WriteLine();
                    Node node = tempKeyword.Nodes.FirstOrDefault(w => w.Number == u);// getURLNodeHasNo(u);
                    temp_Nodes.Add(node);

                    List<int[]> tupple = new List<int[]>();
                    tupple.Add(new int[] { k, u });
                    CurrentConcept++;
                    this.BinaryRelation.MarkAsCovered(tupple, CurrentConcept);
                    AddToCoverage(new OptimalConcept(CurrentConcept, -1, temp_keywords, temp_Nodes));
                    conceptExtracted = true;
                }
                else
                {
                    if (!heighest.IsRectangle())
                    {
                        temp1 = heighest.Clone();
                        temp2 = heighest.Clone();
                        tuple = heighest.convertR_PR(kk, uu);
                        k = kk; u = uu;
                    }
                    else
                    {
                        List<int[]> tup = new List<int[]>();
                        tup.AddRange(heighest.CalculateHeighestTuples());
                        CurrentConcept++;
                        this.BinaryRelation.MarkAsCovered(tup, CurrentConcept);
                        AddToCoverage(heighest.convertToConcept(this.BinaryRelation, CurrentConcept, e));
                        conceptExtracted = true;
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
