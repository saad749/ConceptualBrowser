using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public static class Stemmers
    {
        public static IStemmer GetStemmer(string languageCode)
        {
            switch (languageCode)
            {
                case "eng":
                    return new EnglishStemmer();
                case "fra":
                    return new FrenchStemmer();
                case "spa":
                    return new SpanishStemmer();
                case "ces":
                    return new CzechStemmer();
                case "dan":
                    return new DanishStemmer();
                case "nld":
                    return new DutchStemmer();
                case "fin":
                    return new FinnishStemmer();
                case "deu":
                    return new GermanStemmer();
                case "hun":
                    return new HungarianStemmer();
                case "ita":
                    return new ItalianStemmer();
                case "nor":
                    return new NorwegianStemmer();
                case "por":
                    return new PortugueseStemmer();
                case "ron":
                    return new RomanianStemmer();
                case "rus":
                    return new RussianStemmer();
                case "arb":
                    return new ArabicStemmer();
                default:
                    return null;

            }
        }
    }
}
