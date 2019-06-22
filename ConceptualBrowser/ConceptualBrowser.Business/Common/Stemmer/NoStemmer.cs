using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public class NoStemmer : StemmerOperations, IStemmer
    {
        public string Stem(string input)
        {
            return input;
        }
    }
}