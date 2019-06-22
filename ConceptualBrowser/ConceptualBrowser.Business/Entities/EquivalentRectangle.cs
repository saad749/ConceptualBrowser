using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class EquivalentRectangle
    {
        /// <summary>
        /// Sentence Index, List of KeywordIndices
        /// Sentences with Keywords List
        /// </summary>
        public List<EquivalentNode> Sentences { get; set; } = new List<EquivalentNode>(); //this.urls. will be repalced by this

        /// <summary>
        /// Keyword Index, List of SentenceIndexes
        /// Keywords with Sentences List
        /// </summary>
        public List<EquivalentNode> Keywords { get; set; } = new List<EquivalentNode>();

        public Dictionary<int, HashSet<int>> KeywordsSentencesDictionary { get; set; } = new Dictionary<int, HashSet<int>>();


        public int TupleCount { get; set; }
        public int TotalResults { get; set; }


        //	 create EquivalentRectangle of the BinaryRelation
        /// <summary>
        /// This method adds the sentences for each keyword in the Keywords Equivalent Node List.
        /// </summary>
        /// <param name="binaryRelation"></param>
        public void GetEquivalent(BinaryRelation binaryRelation)
        {
            //Serial Block
            Sentences.Clear();
            for (int i = 0; i < binaryRelation.Keywords.Count; i++)
            {
                KeywordNode keyword = binaryRelation.Keywords[i];
                List<int> tempSentenceIndexes = keyword.Sentences.Select(t => t.SentenceIndex).ToList();
                EquivalentNode nodes = new EquivalentNode(i, tempSentenceIndexes);
                Keywords.Add(nodes);
                //KeywordsSentencesDictionary.Add(i, keyword.SentenceIndexes);
                

            }
            TupleCount = binaryRelation.GetTupleCount();
            TotalResults = binaryRelation.TotalResults;
        }

        //	 create inverse of EquivalentR
        /// <summary>
        /// This method adds the keywords for each sentence in the Sentences Equivalent Node List.
        /// </summary>
        /// <param name="sentences"></param>
        public void GetInverse(List<Sentence> sentences)
        {
            //Parallel Block                                                                                      
            //Sentences.Clear();
            //List<EquivalentNode> sentencesCopy = new List<EquivalentNode>(Sentences);
            //object sync = new object();
            //Parallel.ForEach(sentences, sentence =>
            //{
            //    List<int> tempKeywordIndexes = new List<int>();
            //    tempKeywordIndexes = sentence.KeywordNodes.Select(k => k.KeywordIndex).ToList();
            //    //foreach (EquivalentNode equivalentNode in Keywords)
            //    //{
            //    //    if (equivalentNode.Indexes.Contains(sentence.SentenceIndex))
            //    //        tempKeywordIndexes.Add(equivalentNode.Index);

            //    //}
            //    lock (sync)
            //    {
            //        sentencesCopy.Add(new EquivalentNode(sentence.SentenceIndex, tempKeywordIndexes));
            //    }
            //});
            //Sentences = new List<EquivalentNode>(sentencesCopy);
            //TotalResults = Sentences.Count;

            //SerialBlock
            Sentences.Clear();
            foreach (Sentence sentence in sentences)
            {
                List<int> tempKeywordIndexes = new List<int>();
                //Added Keywords to the Sentence List while creating Binary Relation This avoids going through each keywords and checking all the sentences that 
                //IF a given sentence index is available in the list or not.
                //THis is visible in the line below and the commented out code
                tempKeywordIndexes = sentence.KeywordNodes.Select(k => k.KeywordIndex).ToList();
                //foreach (EquivalentNode equivalentNode in Keywords)
                //{
                //    if (equivalentNode.Indexes.Contains(sentence.SentenceIndex))
                //        tempKeywordIndexes.Add(equivalentNode.Index);
                //}
                this.Sentences.Add(new EquivalentNode(sentence.SentenceIndex, tempKeywordIndexes));
            }
            TotalResults = Sentences.Count;


        }

        //	 convert EquivalentR to elementary relation PR
        public List<int[]> ConvertToElementaryRelation(int keywordIndex, int sentenceIndex)
        {
            List<int> imagesSentenceIndex = GetImagesInverse(sentenceIndex);
            List<int> imagesKeywordIndex = GetImages(keywordIndex);

            //Eliminate rows of inverse(Sentences) not in images of keywords
            for (int i = 0; i < Sentences.Count; i++)
            {
                if (!imagesKeywordIndex.Contains(Sentences[i].Index)) 
                {
                    Sentences.RemoveAt(i);
                    --i;
                }
            }

            //Eliminate rows of Keywords not in images of Sentences
            for (int i = 0; i < Keywords.Count; i++)
            {
                if (!imagesSentenceIndex.Contains(Keywords[i].Index))
                {
                    Keywords.RemoveAt(i);
                    --i;
                }
            }

            //elemenate tuples of R not in images of k
            foreach (EquivalentNode equivalentNode in Keywords)
            {
                for (int i = 0; i < equivalentNode.Indexes.Count; i++)
                {
                    if (!imagesKeywordIndex.Contains(equivalentNode.Indexes[i]))
                    {
                        equivalentNode.Indexes.RemoveAt(i);
                        --i;
                    }
                }
            }

            //elemenate tuples of Inv not in images of u
            foreach (EquivalentNode equivalentNode in Sentences)
            {
                for (int i = 0; i < equivalentNode.Indexes.Count; i++)
                {
                    if (!imagesSentenceIndex.Contains(equivalentNode.Indexes[i]))
                    {
                        equivalentNode.Indexes.RemoveAt(i);
                        --i;
                    }
                }
            }

            TupleCount = Keywords.Sum(w => w.Indexes.Count);
            TotalResults = Sentences.Count;
            return CalculateHighestTuples();
        }

        //	 return the list of nodes associated with Sentences that has index from inverse of EquivalentR
        private List<int> GetImagesInverse(int index)
        {
            return Sentences.FirstOrDefault(s => s.Index == index).Indexes;
        }

        //	 return the list of nodes associated with Keywords that has index from EquivalentR
        private List<int> GetImages(int index)
        {
            return Keywords.FirstOrDefault(k => k.Index == index).Indexes;
        }

        //	 return the list of tuples that is contained in this EquivalentR
        public List<int[]> CalculateHighestTuples()
        {
            List<int[]> tuples = new List<int[]>();
            for (int i = 0; i < Keywords.Count; i++)
            {
                EquivalentNode node = Keywords[i];
                for (int j = 0; j < node.Indexes.Count; j++)
                {
                    int[] pair = { node.Index, node.Indexes[j] };
                    tuples.Add(pair);
                }
            }
            return tuples;
        }

        //	 create copy of this EquivalentR
        public EquivalentRectangle Clone()
        {

            EquivalentRectangle equivalentRectangle = new EquivalentRectangle();
            List<EquivalentNode> tempSentences = new List<EquivalentNode>();

            for (int i = 0; i < Sentences.Count; i++)
            {
                EquivalentNode equivalentNode = Sentences[i].Clone();

                tempSentences.Add(equivalentNode);

            }
            equivalentRectangle.Sentences = tempSentences;

            List<EquivalentNode> tempKeywords = new List<EquivalentNode>();

            for (int i = 0; i < Keywords.Count; i++)
            {
                EquivalentNode equivalentNode = new EquivalentNode();
                equivalentNode = Keywords[i].Clone();
                tempKeywords.Add(equivalentNode);
            }
            equivalentRectangle.Keywords = tempKeywords;
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

        public void Equate(EquivalentRectangle temp)
        {
            Keywords = null;
            Keywords = temp.Keywords;
            Sentences = null;
            Sentences = temp.Sentences;
            TotalResults = temp.TotalResults;
            TupleCount = temp.TupleCount;
        }

        /// <summary>
        /// (TupleCount == Sentences.Count * Keywords.Count) This is the Rectangle definition
        /// </summary>
        /// <returns></returns>
        public bool IsRectangle()
        {
            return (TupleCount == Sentences.Count * Keywords.Count);
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
                    if (sentence.LastCoveredByConceptNumber == currentConceptNo)
                    {
                        //Comparison fixed to avoid redundant addition of sentences to the same concept.
                        if (!tempSentences.Any(x => x.SentenceIndex == sentence.SentenceIndex))
                            tempSentences.Add(sentence);
                    }
                    
                }
            }
            return new OptimalConcept(currentConceptNo, gain, tempKeywords, tempSentences);
        }

    }
}
