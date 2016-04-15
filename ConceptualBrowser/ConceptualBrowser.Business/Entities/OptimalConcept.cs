using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class OptimalConcept
    {
        public int ConceptNumber { get; set; }
        public double Gain { get; set; }
        public List<KeywordNode> Keywords { get; set; }
        public List<Node> Nodes { get; set; }
        public NodeTree NodesTree { get; set; } = null;
        public string ConceptName { get; set; }

        public OptimalConcept(int conceptNumber, double gain, List<KeywordNode> keywords, List<Node> nodes)
        {
            ConceptNumber = conceptNumber;
            Gain = gain;
            Keywords = keywords;
            Nodes = nodes;
            NodesTree = new NodeTree(nodes.Count);
        }

        public void Model()
        {
            int position = 0;
            for (int i = 0; i < Nodes.Count; i++)
            {
                if (!this.InHeap(Nodes[i]))
                {
                    if (NodesTree.IsNewElement)
                        NodesTree.InsertNode(Nodes[i], position++);
                    else
                        NodesTree.InsertNode(Nodes[i], position - 1);
                }
            }
        }

        // check whether this concept is already in the heap
        private bool InHeap(Node node)
        {
            for (int i = 0; i < NodesTree.Heap.Count; i++)
            {
                for (int j = 0; j < NodesTree.Heap[i].Count; j++)
                {
                    Node tempNode = NodesTree.Heap[i][j];
                    if (tempNode.Word.Equals(node.Word,StringComparison.OrdinalIgnoreCase))
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
                        index = Keywords[i].Number;
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
                index = Keywords[0].Number;
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
                            index = keyword.Number;
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
