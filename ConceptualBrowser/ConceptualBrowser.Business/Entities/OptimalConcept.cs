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
        [JsonIgnore]
        public List<KeywordNode> Keywords { get; set; }
        [JsonIgnore]
        public NodeTree NodesTree { get; set; } = null;

        public OptimalConcept(int conceptNumber, double gain, List<KeywordNode> keywords, List<Sentence> sentences)
        {
            ConceptNumber = conceptNumber;
            Gain = gain;
            Keywords = keywords;
            Sentences = sentences;
            NodesTree = new NodeTree(sentences.Count);
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
