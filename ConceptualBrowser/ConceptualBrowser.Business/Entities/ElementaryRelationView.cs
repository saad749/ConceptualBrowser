using System;
using System.Collections.Generic;
using System.Linq;

namespace ConceptualBrowser.Business.Entities
{
    /// <summary>
    /// PHASE 5 OPTIMIZATION: Non-destructive view of an EquivalentRectangle
    /// for elementary relation operations without expensive cloning
    /// </summary>
    public class ElementaryRelationView
    {
        private readonly EquivalentRectangle _source;
        private readonly HashSet<int> _validSentenceIndices;
        private readonly HashSet<int> _validKeywordIndices;
        private readonly int _keywordIndex;
        private readonly int _sentenceIndex;

        // Cached results for performance
        private double? _cachedEconomy = null;
        private List<int[]> _cachedTuples = null;
        private int? _cachedTupleCount = null;
        private int? _cachedSentenceCount = null;
        private int? _cachedKeywordCount = null;

        public ElementaryRelationView(EquivalentRectangle source, int keywordIndex, int sentenceIndex)
        {
            _source = source ?? throw new ArgumentNullException(nameof(source));
            _keywordIndex = keywordIndex;
            _sentenceIndex = sentenceIndex;

            // Calculate filtered indices based on images (without modifying source)
            var imagesSentenceIndex = GetImagesInverse(sentenceIndex);
            var imagesKeywordIndex = GetImages(keywordIndex);

            // Create HashSets for O(1) lookups
            _validSentenceIndices = new HashSet<int>(imagesSentenceIndex);
            _validKeywordIndices = new HashSet<int>(imagesKeywordIndex);
        }

        /// <summary>
        /// Calculate economy for this filtered view without modifying the source
        /// </summary>
        public double CalculateEconomy()
        {
            if (_cachedEconomy.HasValue)
                return _cachedEconomy.Value;

            int tupleCount = GetTupleCount();
            int sentenceCount = GetSentenceCount();
            int keywordCount = GetKeywordCount();

            if (sentenceCount == 0 || keywordCount == 0)
            {
                _cachedEconomy = -1.0; // Invalid relation
                return _cachedEconomy.Value;
            }

            double num1 = (double)tupleCount / (double)(sentenceCount * keywordCount);
            double dem = tupleCount - (sentenceCount + keywordCount);
            _cachedEconomy = num1 * dem;

            return _cachedEconomy.Value;
        }

        /// <summary>
        /// Get tuples for this filtered view
        /// </summary>
        public List<int[]> CalculateHighestTuples()
        {
            if (_cachedTuples != null)
                return _cachedTuples;

            _cachedTuples = new List<int[]>();

            foreach (var keywordNode in _source.Keywords)
            {
                if (!_validKeywordIndices.Contains(keywordNode.Index))
                    continue;

                foreach (int sentenceIdx in keywordNode.Indexes)
                {
                    if (_validSentenceIndices.Contains(sentenceIdx))
                    {
                        int[] pair = { keywordNode.Index, sentenceIdx };
                        _cachedTuples.Add(pair);
                    }
                }
            }

            return _cachedTuples;
        }

        /// <summary>
        /// Check if this view represents a rectangle
        /// </summary>
        public bool IsRectangle()
        {
            int tupleCount = GetTupleCount();
            int sentenceCount = GetSentenceCount();
            int keywordCount = GetKeywordCount();

            return tupleCount == (sentenceCount * keywordCount);
        }

        /// <summary>
        /// Get count of tuples in this filtered view
        /// </summary>
        public int GetTupleCount()
        {
            if (_cachedTupleCount.HasValue)
                return _cachedTupleCount.Value;

            int count = 0;
            foreach (var keywordNode in _source.Keywords)
            {
                if (_validKeywordIndices.Contains(keywordNode.Index))
                {
                    count += keywordNode.Indexes.Count(idx => _validSentenceIndices.Contains(idx));
                }
            }

            _cachedTupleCount = count;
            return count;
        }

        /// <summary>
        /// Get count of sentences in this filtered view
        /// </summary>
        public int GetSentenceCount()
        {
            if (_cachedSentenceCount.HasValue)
                return _cachedSentenceCount.Value;

            _cachedSentenceCount = _source.Sentences.Count(s => _validSentenceIndices.Contains(s.Index));
            return _cachedSentenceCount.Value;
        }

        /// <summary>
        /// Get count of keywords in this filtered view
        /// </summary>
        public int GetKeywordCount()
        {
            if (_cachedKeywordCount.HasValue)
                return _cachedKeywordCount.Value;

            _cachedKeywordCount = _source.Keywords.Count(k => _validKeywordIndices.Contains(k.Index));
            return _cachedKeywordCount.Value;
        }

        /// <summary>
        /// Get list of nodes associated with Sentences that has index from inverse of EquivalentR
        /// </summary>
        private List<int> GetImagesInverse(int index)
        {
            var sentence = _source.Sentences.FirstOrDefault(s => s.Index == index);
            return sentence?.Indexes ?? new List<int>();
        }

        /// <summary>
        /// Get list of nodes associated with Keywords that has index from EquivalentR
        /// </summary>
        private List<int> GetImages(int index)
        {
            var keyword = _source.Keywords.FirstOrDefault(k => k.Index == index);
            return keyword?.Indexes ?? new List<int>();
        }

        /// <summary>
        /// Create a filtered EquivalentRectangle for compatibility with existing code
        /// Only use when absolutely necessary - prefer using view methods directly
        /// </summary>
        public EquivalentRectangle ToEquivalentRectangle()
        {
            var result = new EquivalentRectangle();

            // Filter sentences
            result.Sentences = _source.Sentences
                .Where(s => _validSentenceIndices.Contains(s.Index))
                .Select(s => new EquivalentNode(s.Index, s.Indexes.Where(idx => _validKeywordIndices.Contains(idx)).ToList()))
                .ToList();

            // Filter keywords
            result.Keywords = _source.Keywords
                .Where(k => _validKeywordIndices.Contains(k.Index))
                .Select(k => new EquivalentNode(k.Index, k.Indexes.Where(idx => _validSentenceIndices.Contains(idx)).ToList()))
                .ToList();

            result.TupleCount = GetTupleCount();
            result.SentenceCount = GetSentenceCount();

            return result;
        }
    }
}