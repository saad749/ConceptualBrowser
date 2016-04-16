﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class EquivalentRectangle
    {
        public List<EquivalentNode> EquivalentNodes { get; set; } = new List<EquivalentNode>(); //this.urls. will be repalced by this
        public List<EquivalentNode> Keywords { get; set; } = new List<EquivalentNode>();
        public int TupleCount { get; set; }
        public int TotalResults { get; set; }

        //public EquivalentRectangle(List<Node> nodes, List<KeywordNode> keywords, int tupleCount, int totalResults)  //Not needed Probably....
        //{
        //    Nodes.AddRange(nodes);
        //    Keywords.AddRange(keywords);
        //    TupleCount = tupleCount;
        //    TotalResults = totalResults;
        //}

        //	 create EquivalentRectangle of the BinaryRelation
        /// <summary>
        /// This method adds the sentences for each keyword in the Keywords Equivalent Node List.
        /// </summary>
        /// <param name="binaryRelation"></param>
        public void GetEquivalent(BinaryRelation binaryRelation)
        {
            EquivalentNodes.Clear();
            for (int i = 0; i < binaryRelation.Keywords.Count; i++)
            {
                KeywordNode keyword = binaryRelation.Keywords[i];
                List<int> tempSentenceIndexes = keyword.Sentences.Select(t => t.SentenceIndex).ToList();
                EquivalentNode nodes = new EquivalentNode(i, tempSentenceIndexes);
                Keywords.Add(nodes);
            }
            TupleCount = binaryRelation.GetTupleCount();
            TotalResults = binaryRelation.TotalResults;
        }

        //	 create inverse of EquivalentR
        public void GetInverse(List<Sentence> sentences)
        {
            EquivalentNodes.Clear();
            //Sentence sentence = new Sentence();

            foreach (Sentence sentence in sentences)
            {
                //sentence = sentence;
                List<int> tempKeywordIndexes = new List<int>();

                foreach (EquivalentNode equivalentNode in Keywords)
                {
                    //if (equivalentNode.InNode(sentence.SentenceIndex))
                    if (equivalentNode.SentenceIndexes.Contains(sentence.SentenceIndex))
                        tempKeywordIndexes.Add(equivalentNode.Index);
                }
                //EquivalentNode equivalentNode1 = new EquivalentNode(sentence.SentenceIndex, tempKeywords);
                this.EquivalentNodes.Add(new EquivalentNode(sentence.SentenceIndex, tempKeywordIndexes));
            }

            TotalResults = EquivalentNodes.Count;
        }

        //	 convert EquivalentR to elementary relation PR
        public List<int[]> convertR_PR(int k, int u)
        {

            List<int> images_u = new List<int>();
            List<int> images_k = new List<int>();
            images_u = GetImagesInverse(u);
            images_k = GetImages(k);

            //elemenate rows of inv not in images of k
            for (int i = 0; i < EquivalentNodes.Count; i++)
            {
                List<EquivalentNode> equivalentNodes = EquivalentNodes;
                EquivalentNode tmp = equivalentNodes[i];
                int index = tmp.Index;
                if (!images_k.Contains(index))
                {
                    EquivalentNodes.RemoveAt(i);
                    --i;
                }
            }

            //elemenate rows of R not in images of u
            for (int t = 0; t < Keywords.Count; t++)
            {
                int index = Keywords[t].Index;
                if (!images_u.Contains((index)))
                {
                    Keywords.RemoveAt(t);
                    --t;
                }
            }

            //elemenate tuples of R not in images of k
            for (int i = 0; i < Keywords.Count; i++)
            {
                EquivalentNode node = Keywords[i];
                List<int> nodes = node.SentenceIndexes;
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (!images_k.Contains(nodes[j]))
                    {
                        nodes.RemoveAt(j);
                        --j;
                    }
                }
            }

            //elemenate tuples of Inv not in images of u
            for (int i = 0; i < EquivalentNodes.Count; i++)
            {
                EquivalentNode node = EquivalentNodes[i];
                List<int> nodes = node.SentenceIndexes;
                for (int j = 0; j < nodes.Count; j++)
                {
                    if (!images_u.Contains(nodes[j]))
                    {
                        nodes.RemoveAt(j);
                        --j;
                    }
                }
            }
            TupleCount = Keywords.Sum(w => w.SentenceIndexes.Count);
            TotalResults = EquivalentNodes.Count;
            return CalculateHighestTuples();
        }

        //	 return the list of nodes associated with EquivalentNode that has index from inverse of EquivalentR
        private List<int> GetImagesInverse(int index)
        {
            EquivalentNode node = GetAtIndex(EquivalentNodes, index);
            return node.SentenceIndexes;
        }

        //	 return EquivalentNode that has index from the list l
        public EquivalentNode GetAtIndex(List<EquivalentNode> nodes, int index)
        {
            EquivalentNode node = new EquivalentNode();

            for (int i = 0; i < nodes.Count; i++)
            {
                node = nodes[i];
                if (node.Index == index)
                    return node;
            }
            return null;
        }

        //	 return the list of nodes associated with EquivalentNode that has index from EquivalentR
        private List<int> GetImages(int index)
        {
            EquivalentNode node = new EquivalentNode();
            node = GetAtIndex(Keywords, index);
            return node.SentenceIndexes;
        }

        //	 return the list of tuples that is contained in this EquivalentR
        public List<int[]> CalculateHighestTuples()
        {
            List<int[]> tuples = new List<int[]>();
            for (int i = 0; i < Keywords.Count; i++)
            {
                EquivalentNode node = Keywords[i];
                for (int j = 0; j < node.SentenceIndexes.Count; j++)
                {
                    int[] pair = { node.Index, node.SentenceIndexes[j] };
                    tuples.Add(pair);
                }
            }
            return tuples;
        }

        //	 create copy of this EquivalentR
        public EquivalentRectangle Clone()
        {

            EquivalentRectangle equivalentRectangle = new EquivalentRectangle();
            List<EquivalentNode> temp_nodes = new List<EquivalentNode>();

            for (int i = 0; i < EquivalentNodes.Count; i++)
            {
                EquivalentNode equivalentNode = EquivalentNodes[i].Clone();

                temp_nodes.Add(equivalentNode);

            }
            equivalentRectangle.EquivalentNodes = temp_nodes;

            List<EquivalentNode> temp_nodes_k = new List<EquivalentNode>();

            for (int i = 0; i < Keywords.Count; i++)
            {
                EquivalentNode equivalentNode = new EquivalentNode();
                equivalentNode = Keywords[i].Clone();
                temp_nodes_k.Add(equivalentNode);
            }
            equivalentRectangle.Keywords = temp_nodes_k;
            TupleCount = equivalentRectangle.TupleCount;
            TotalResults = equivalentRectangle.TotalResults;

            return equivalentRectangle;
        }

        // calculate economy of the extracted concept
        // (r/(c*d))*(r-(c+d))
        public double CalculateEconomy(BinaryRelation binaryRelation)
        {
            double num1 = ((double)TupleCount/ (double)(TotalResults * Keywords.Count));
            double dem = (TupleCount - (TotalResults + Keywords.Count));
            return (num1 * dem);
        }

        public void equate(EquivalentRectangle temp)
        {
            Keywords = null;
            Keywords = temp.Keywords;
            EquivalentNodes = null;
            EquivalentNodes = temp.EquivalentNodes;
            TotalResults = temp.TotalResults;
            TupleCount = temp.TupleCount;
        }

        public bool IsRectangle()
        {
            return (TupleCount == EquivalentNodes.Count * Keywords.Count);
        }

        //	 convert this EquivalentR to object of type OptimalConcept
        public OptimalConcept ConvertToConcept(BinaryRelation binaryRelation, int currentConceptNo, double gain)
        {
            //Console.WriteLine();
            //Console.WriteLine("ConceptNumber: " + currentConceptNo);
            //Console.WriteLine();
            List<KeywordNode> tempKeywords = new List<KeywordNode>();
            for (int i = 0; i < Keywords.Count; i++)
            {
                int index = Keywords[i].Index;
                KeywordNode x = binaryRelation.Keywords[index];
                tempKeywords.Add(x);
            }

            List<Sentence> tempSentences = new List<Sentence>();
            for (int i = 0; i < tempKeywords.Count; i++)
            {
                KeywordNode keywordNode = tempKeywords[i];
                for (int j = 0; j < keywordNode.Sentences.Count; j++)
                {
                    Sentence sentence = keywordNode.Sentences[j];
                    if (sentence.CoveredByConceptNumber == currentConceptNo)
                        if (!tempSentences.Contains(sentence))
                            tempSentences.Add(sentence);
                }
            }
            return new OptimalConcept(currentConceptNo, gain, tempKeywords, tempSentences);
        }
    }
}
