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

        // PERFORMANCE OPTIMIZATION: Cache rectangle properties for faster detection
        private bool? _isRectangleCache = null;
        private bool _cacheValid = false;
        private int _lastTupleCount = -1;
        private int _lastProductCount = -1;

        // PHASE 4 OPTIMIZATION: Cache economy calculation results
        private double? _economyCache = null;
        private int _economyCacheTupleCount = -1;
        private int _economyCacheSentenceCount = -1;
        private int _economyCacheKeywordsCount = -1;

        public int TupleCount { get; set; }
        public int SentenceCount { get; set; }


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
            SentenceCount = binaryRelation.TotalResults;
            InvalidateCache(); // PERFORMANCE OPTIMIZATION: Invalidate cache after structure change
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
            SentenceCount = Sentences.Count;
            InvalidateCache(); // PERFORMANCE OPTIMIZATION: Invalidate cache after structure change


        }

        //	 convert EquivalentR to elementary relation PR
        public List<int[]> ConvertToElementaryRelation(int keywordIndex, int sentenceIndex)
        {
            List<int> imagesSentenceIndex = GetImagesInverse(sentenceIndex);
            List<int> imagesKeywordIndex = GetImages(keywordIndex);

            // PHASE 6 OPTIMIZATION: Convert to HashSets for O(1) lookups
            var imagesSentenceSet = new HashSet<int>(imagesSentenceIndex);
            var imagesKeywordSet = new HashSet<int>(imagesKeywordIndex);

            //Eliminate rows of inverse(Sentences) not in images of keywords
            for (int i = 0; i < Sentences.Count; i++)
            {
                if (!imagesKeywordSet.Contains(Sentences[i].Index))
                {
                    Sentences.RemoveAt(i);
                    --i;
                }
            }

            //Eliminate rows of Keywords not in images of Sentences
            for (int i = 0; i < Keywords.Count; i++)
            {
                if (!imagesSentenceSet.Contains(Keywords[i].Index))
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
                    if (!imagesKeywordSet.Contains(equivalentNode.Indexes[i]))
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
                    if (!imagesSentenceSet.Contains(equivalentNode.Indexes[i]))
                    {
                        equivalentNode.Indexes.RemoveAt(i);
                        --i;
                    }
                }
            }

            TupleCount = Keywords.Sum(w => w.Indexes.Count);
            SentenceCount = Sentences.Count;
            InvalidateCache(); // PERFORMANCE OPTIMIZATION: Invalidate cache after structure change
            return CalculateHighestTuples();
        }

        //	 return the list of nodes associated with Sentences that has index from inverse of EquivalentR
        private List<int> GetImagesInverse(int index)
        {
            var sentence = Sentences.FirstOrDefault(s => s.Index == index);
            return sentence?.Indexes ?? new List<int>();
        }

        //	 return the list of nodes associated with Keywords that has index from EquivalentR
        private List<int> GetImages(int index)
        {
            var keyword = Keywords.FirstOrDefault(k => k.Index == index);
            return keyword?.Indexes ?? new List<int>();
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
                EquivalentNode equivalentNode = Keywords[i].Clone();
                tempKeywords.Add(equivalentNode);
            }
            equivalentRectangle.Keywords = tempKeywords;
            TupleCount = equivalentRectangle.TupleCount;
            SentenceCount = equivalentRectangle.SentenceCount;

            return equivalentRectangle;
        }

        // calculate economy of the extracted concept
        // W (R) = (r / dc) (r - (d + c))
        // where r is the cardinal of R (i.e., the number of pairs in R)
        // d is the cardinal of the domain of R.
        // c is the cardinal of the range of R.
        // Remark: The quantity(r / dc) provides a measure of the density of the relation R.
        // The quantity(r - (d + c)) is a measure of the economy of information.
        public double CalculateEconomy()
        {
            // PHASE 4 OPTIMIZATION: Use cached result if available and valid
            if (_economyCache.HasValue &&
                _economyCacheTupleCount == TupleCount &&
                _economyCacheSentenceCount == SentenceCount &&
                _economyCacheKeywordsCount == Keywords.Count)
            {
                return _economyCache.Value;
            }

            // Calculate and cache the result
            double num1 = ((double)TupleCount / (double)(SentenceCount * Keywords.Count));
            double dem = (TupleCount - (SentenceCount + Keywords.Count));
            double result = num1 * dem;

            // Update cache
            _economyCache = result;
            _economyCacheTupleCount = TupleCount;
            _economyCacheSentenceCount = SentenceCount;
            _economyCacheKeywordsCount = Keywords.Count;

            return result;
        }

        public void Equate(EquivalentRectangle temp)
        {
            Keywords = null;
            Keywords = temp.Keywords;
            Sentences = null;
            Sentences = temp.Sentences;
            SentenceCount = temp.SentenceCount;
            TupleCount = temp.TupleCount;
            InvalidateCache(); // PERFORMANCE OPTIMIZATION: Invalidate cache after structure change
        }

        /// <summary>
        /// PERFORMANCE OPTIMIZATION: Cached rectangle detection for faster repeated checks
        /// (TupleCount == Sentences.Count * Keywords.Count) This is the Rectangle definition
        /// </summary>
        /// <returns></returns>
        public bool IsRectangle()
        {
            int currentProductCount = Sentences.Count * Keywords.Count;

            // Check if cache is valid
            if (_cacheValid && _lastTupleCount == TupleCount && _lastProductCount == currentProductCount)
            {
                return _isRectangleCache.Value;
            }

            // Calculate and cache result
            bool isRectangle = (TupleCount == currentProductCount);
            _isRectangleCache = isRectangle;
            _lastTupleCount = TupleCount;
            _lastProductCount = currentProductCount;
            _cacheValid = true;

            return isRectangle;
        }

        /// <summary>
        /// PERFORMANCE OPTIMIZATION: Invalidate all caches when structure changes
        /// </summary>
        private void InvalidateCache()
        {
            _cacheValid = false;
            _isRectangleCache = null;
            // PHASE 4: Also clear economy cache
            _economyCache = null;
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
