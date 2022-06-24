using Snowball;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public static class Stemmers
    {
        public static IStemmer GetStemmer(string languageCode)
        {
            switch (languageCode)
            {
                case "arb":
                    return new ArabicStemmer();
                case "dan":
                    return new DanishStemmer();
                case "nld":
                    return new DutchStemmer();
                case "eng":
                    return new EnglishStemmer();
                case "fin":
                    return new FinnishStemmer();
                case "fra":
                    return new FrenchStemmer();
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
                case "spa":
                    return new SpanishStemmer();
                case "none":
                    return new NoStemmer();
                default:
                    return null;

            }
        }
    }
}
