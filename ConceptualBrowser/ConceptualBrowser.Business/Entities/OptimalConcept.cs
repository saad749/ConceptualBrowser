using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class OptimalConcept
    {
        public string ConceptName { get; set; }
        public int ConceptNumber { get; set; }
        public double Gain { get; set; }
        public List<Sentence> Sentences { get; set; }
        public List<KeywordNode> Keywords { get; set; }

        // Category support: Lazy-computed from sentence categories
        private string _category = null;
        private int? _positiveCount = null;
        private int? _negativeCount = null;

        /// <summary>
        /// Category of this concept based on majority vote of its sentences.
        /// Returns "positive" if majority of sentences are positive, "negative" otherwise.
        /// Defaults to "negative" on ties. Returns null if no category data available.
        /// </summary>
        public string Category
        {
            get
            {
                if (_category == null && Sentences != null && Sentences.Count > 0)
                {
                    CalculateCategoryStats();
                }
                return _category;
            }
        }

        /// <summary>
        /// Count of positive sentences in this concept (lazy-computed).
        /// </summary>
        public int PositiveCount
        {
            get
            {
                if (_positiveCount == null && Sentences != null)
                {
                    CalculateCategoryStats();
                }
                return _positiveCount ?? 0;
            }
        }

        /// <summary>
        /// Count of negative sentences in this concept (lazy-computed).
        /// </summary>
        public int NegativeCount
        {
            get
            {
                if (_negativeCount == null && Sentences != null)
                {
                    CalculateCategoryStats();
                }
                return _negativeCount ?? 0;
            }
        }

        /// <summary>
        /// Percentage of the dominant category (0-100).
        /// Returns the percentage of positive sentences if Category is "Positive",
        /// or percentage of negative sentences if Category is "Negative".
        /// Returns 0 if no category data available.
        /// </summary>
        public double CategoryPercentage
        {
            get
            {
                int total = PositiveCount + NegativeCount;
                if (total == 0)
                    return 0;

                int dominantCount = Category == "Positive" ? PositiveCount : NegativeCount;
                return Math.Round((double)dominantCount / total * 100, 2);
            }
        }

        /// <summary>
        /// Percentage of positive sentences (0-100).
        /// </summary>
        public double PositivePercentage
        {
            get
            {
                int total = PositiveCount + NegativeCount;
                if (total == 0)
                    return 0;
                return Math.Round((double)PositiveCount / total * 100, 2);
            }
        }

        /// <summary>
        /// Percentage of negative sentences (0-100).
        /// </summary>
        public double NegativePercentage
        {
            get
            {
                int total = PositiveCount + NegativeCount;
                if (total == 0)
                    return 0;
                return Math.Round((double)NegativeCount / total * 100, 2);
            }
        }

        /// <summary>
        /// Calculates category statistics from sentences.
        /// Called lazily when Category, PositiveCount, or NegativeCount is first accessed.
        /// </summary>
        private void CalculateCategoryStats()
        {
            if (Sentences == null || Sentences.Count == 0)
            {
                _positiveCount = 0;
                _negativeCount = 0;
                _category = null;
                return;
            }

            // Check if any sentence has category data
            bool hasCategoryData = Sentences.Any(s => !string.IsNullOrEmpty(s.Category));
            if (!hasCategoryData)
            {
                _positiveCount = 0;
                _negativeCount = 0;
                _category = null;
                return;
            }

            // Count positive and negative sentences
            _positiveCount = Sentences.Count(s => s.IsPositive);
            _negativeCount = Sentences.Count - _positiveCount.Value;

            // Majority vote with tie-breaker defaulting to negative
            _category = _positiveCount > _negativeCount ? "Positive" : "Negative";
        }

        // PHASE 4 OPTIMIZATION: Lazy initialization to save memory
        private NodeTree _nodesTree = null;
        [JsonIgnore]
        public NodeTree NodesTree
        {
            get
            {
                if (_nodesTree == null && Sentences != null)
                {
                    _nodesTree = new NodeTree(Sentences.Count);
                }
                return _nodesTree;
            }
            set { _nodesTree = value; }
        }

        public OptimalConcept(int conceptNumber, double gain, List<KeywordNode> keywords, List<Sentence> sentences)
        {
            ConceptNumber = conceptNumber;
            Gain = gain;
            Keywords = keywords;
            Sentences = sentences;
            // PHASE 4 OPTIMIZATION: Defer NodeTree creation until actually needed
            // NodesTree = new NodeTree(sentences.Count); // Removed - will be created on demand
        }

        public void Model()
        {
            int position = 0;
            for (int i = 0; i < Sentences.Count; i++)
            {
                if (!this.InHeap(Sentences[i]))
                {
                    if (NodesTree.IsNewElement)
                        NodesTree.InsertNode(Sentences[i], position++);
                    else
                        NodesTree.InsertNode(Sentences[i], position - 1);
                }
            }
        }

        // check whether this concept is already in the heap
        private bool InHeap(Sentence sentence)
        {
            for (int i = 0; i < NodesTree.Heap.Count; i++)
            {
                for (int j = 0; j < NodesTree.Heap[i].Count; j++)
                {
                    Sentence tempNode = NodesTree.Heap[i][j];
                    if (tempNode.SentenceIndex == sentence.SentenceIndex)
                        return true;
                }
            }
            return false;
        }

        // name the concept
        public void SetConceptName(BinaryRelation binaryRelation)
        {
            double min = -1; //min keyword rank
            int index = -1;   // min rank keyword no
            //String key = null; // min rank keyword no
            String rootString = null; //min rank keyword root string
            String keywordString = null; //min rank keyword original string
            bool found = false;  //min rank keyword no
            for (int i = 0; i < Keywords.Count; i++)
            {
                if (!found)
                {
                    String root = Keywords[i].Keyword;
                    if (!binaryRelation.GetRootNode(root).Covered)
                    {
                        min = Keywords[i].KeywordRank;
                        index = Keywords[i].KeywordIndex;
                        rootString = Keywords[i].Keyword;

                        if (binaryRelation.InPrimaryConceptsName(rootString))
                            keywordString = (binaryRelation.GetRootNode(rootString)).getMaxLengthWord() + " Advance";
                        else
                            keywordString = (binaryRelation.GetRootNode(rootString)).getMaxLengthWord();

                        found = true;
                    }
                }
            }


            if (!found)
            {
                binaryRelation.ResetRootNodes();
                min = Keywords[0].KeywordRank;
                index = Keywords[0].KeywordIndex;
                rootString = Keywords[0].Keyword;

                if (binaryRelation.InPrimaryConceptsName(rootString))
                    keywordString = (binaryRelation.GetRootNode(rootString)).getMaxLengthWord() + " Advance";
                else
                    keywordString = (binaryRelation.GetRootNode(rootString)).getMaxLengthWord();
            }
            else
            {
                for (int i = 0; i < Keywords.Count; i++)
                {
                    KeywordNode keyword = Keywords[i];
                    if (!((RootNode)binaryRelation.GetRootNode(keyword.Keyword)).Covered)
                        if (keyword.KeywordRank < min)
                        {
                            index = keyword.KeywordIndex;
                            min = keyword.KeywordRank;
                            rootString = keyword.Keyword;

                            if (binaryRelation.InPrimaryConceptsName(rootString))
                                keywordString = (binaryRelation.GetRootNode(rootString)).getMaxLengthWord() + " Advance";
                            else
                                keywordString = (binaryRelation.GetRootNode(rootString)).getMaxLengthWord();
                        }
                }
            }

            ((RootNode)binaryRelation.GetRootNode(rootString)).Covered = true;
            binaryRelation.PrimaryConceptsName.Add(rootString);
            ConceptName = keywordString;

        }

    }
}
