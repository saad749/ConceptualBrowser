using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    public class Rank
    {
        public int RankLength { get; set; }

        public int[] Ranks { get; set; }

        public int[] TotalResults { get; set; }

        public double CalRank { get; set; } = 0; //C# 6.0 allows property initializers

        public Rank(int rankLength, int[] ranks, int[] totalResults)
        {
            RankLength = rankLength;
            Ranks = ranks;
            TotalResults = totalResults;

        }
        public void CalculateRank()
        {
            double sum = 0;
            double sizes = 0;
            for (int i = 0; i < RankLength; i++)
            {
                sum += Ranks[i] * TotalResults[i];
                sizes += TotalResults[i];
            }
            CalRank = ((double)sum / sizes);
        }
    }
}
