using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class KeywordNode
    {
        /// <summary>
        /// KeywordIndex in the list of Keywords. A number assigned to the word (probably) related to chronological order of appearance.
        /// </summary>
        public int KeywordIndex { get; set; } //Number of Keyword Node
        /// <summary>
        /// Word Stem of the Words this Keyword Represents
        /// </summary>
        public string Keyword { get; set; } // Name of keywordNode
        /// <summary>
        /// Number of times this Keyword appeared in the text.
        /// </summary>
        public double KeywordRank { get; set; }

        /// <summary>
        /// PERFORMANCE OPTIMIZATION: Use indexes instead of full sentence objects to reduce memory usage
        /// </summary>
        public HashSet<int> SentenceIndexes { get; set; } = new HashSet<int>();

        /// <summary>
        /// Sentences in which the word occured (DEPRECATED: Use SentenceIndexes for memory efficiency)
        /// </summary>
        [JsonIgnore]
        [Obsolete("Use SentenceIndexes for better memory efficiency")]
        public List<Sentence> Sentences { get; set; } = new List<Sentence>();// list of words associated with this KeywordNode

        public KeywordNode(string keyWord, int number, int rank, List<Sentence> sentences)
        {
            Keyword = keyWord;
            KeywordIndex = number;
            KeywordRank = rank;
            // PERFORMANCE OPTIMIZATION: Store both for backward compatibility during transition
            Sentences = sentences;
            SentenceIndexes = new HashSet<int>(sentences.Select(s => s.SentenceIndex));
        }

        public bool ContainsSentenceIndex(int sentenceIndex)
        {
            // PERFORMANCE OPTIMIZATION: Use O(1) HashSet lookup instead of O(n) LINQ query
            return SentenceIndexes.Contains(sentenceIndex);
        }
    }
}
