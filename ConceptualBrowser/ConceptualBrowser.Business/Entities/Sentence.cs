using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class Sentence
    {
        public int SentenceIndex { get; set; } //Renamed from Word to SentenceIndex
        public int CoveredByConceptNumber { get; set; } = 1;
        public Rank Rank { get; set; }
        public int KeywordNumber { get; set; }
        public string KeywordString { get; set; }


        public Sentence()
        {

        }

        public Sentence(int sentenceIndex, int coveredByConceptNumber, Rank rank)
        {
            SentenceIndex = sentenceIndex;
            CoveredByConceptNumber = coveredByConceptNumber;
            Rank = rank;
            rank.CalculateRank();
        }
    }
}
