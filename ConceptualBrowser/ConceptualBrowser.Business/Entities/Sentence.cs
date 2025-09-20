using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class Sentence
    {
        public int SentenceIndex { get; set; }                                          //Renamed from Word to SentenceIndex
        public int LastCoveredByConceptNumber { get; set; } = -1;                       // A signle sentence can be covered by multiple concepts; Why 1 only??
        public List<int> CovertedbyConceptNumbers { get; set; } = new List<int>();
        public int KeywordNumber { get; set; }
        public string KeywordString { get; set; }
        public string OriginalSentence { get; set; }

        /// <summary>
        /// PERFORMANCE OPTIMIZATION: Use indexes instead of full keyword objects to reduce memory usage
        /// </summary>
        public HashSet<int> KeywordIndexes { get; set; } = new HashSet<int>();

        [JsonIgnore]
        [Obsolete("Use KeywordIndexes for better memory efficiency")]
        public List<KeywordNode> KeywordNodes { get; set; }
        [JsonIgnore]
        public Rank Rank { get; set; }
        


        public Sentence()
        {

        }

        public Sentence(int sentenceIndex, int coveredByConceptNumber, Rank rank, string originalSentence)
        {
            SentenceIndex = sentenceIndex;
            LastCoveredByConceptNumber = coveredByConceptNumber;
            OriginalSentence = originalSentence.Trim();
            Rank = rank;
            rank.CalculateRank();
        }

        /// <summary>
        /// Constructor to Allow cross referencing the keywords that occur in this sentence
        /// </summary>
        /// <param name="sentenceIndex"></param>
        /// <param name="coveredByConceptNumber"></param>
        /// <param name="rank"></param>
        /// <param name="keywords"></param>
        public Sentence(int sentenceIndex, int coveredByConceptNumber, Rank rank, List<KeywordNode> keywords, string originalSentence)
        {
            SentenceIndex = sentenceIndex;
            LastCoveredByConceptNumber = coveredByConceptNumber;
            OriginalSentence = originalSentence.Trim();
            Rank = rank;
            rank.CalculateRank();
            // PERFORMANCE OPTIMIZATION: Store both for backward compatibility during transition
            KeywordNodes = keywords;
            KeywordIndexes = new HashSet<int>(keywords?.Select(k => k.KeywordIndex) ?? new List<int>());
        }
    }
}
