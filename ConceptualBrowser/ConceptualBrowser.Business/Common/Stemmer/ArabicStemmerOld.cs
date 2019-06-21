using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public class ArabicStemmerOld : StemmerOperations, IStemmer
    {
        public string Stem(string input)
        {
            string[] prefixes = { "ل", "لل", "وس", "ول", "و" };
            string[] suffixes = { "هما", "تما", "كما", "ان", "ها", "وا", "تم", "كم", "تن", "كن", "نا", "تا", "ته", "ما", "ون", " ين", "هن", "هم", "ته", "تي", "ني", "ا", "ي", "ات", "ت", "ه" };

            foreach (var prefix in prefixes)
            {
                if (input.StartsWith(prefix))
                {
                    input = input.Substring(0, prefix.Length);
                    break;
                }

            }

            foreach (var suffix in suffixes)
            {
                if (input.StartsWith(suffix))
                {
                    input = input.Substring(input.Length - suffix.Length, suffix.Length);
                    break;
                }

            }

            return input;
        }
    }
}