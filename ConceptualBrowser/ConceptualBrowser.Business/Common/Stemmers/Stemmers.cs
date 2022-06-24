using Snowball;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public static class Stemmers
    {
        public static IStemmer GetStemmer(string languageCode)
        {
            switch (languageCode)
            {
                case "ara":
                    return new ArabicStemmer();
                case "hye":
                    return new ArmenianStemmer();
                case "eus":
                    return new BasqueStemmer();
                case "cat":
                    return new CatalanStemmer();
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
                case "ell":
                    return new GreekStemmer();
                case "hin":
                    return new HindiStemmer();
                case "hun":
                    return new HungarianStemmer();
                case "ind":
                    return new IndonesianStemmer();
                case "gle":
                    return new IrishStemmer();
                case "ita":
                    return new ItalianStemmer();
                case "lit":
                    return new LithuanianStemmer();
                case "nep":
                    return new NepaliStemmer();
                case "nor":
                    return new NorwegianStemmer();
                case "por":
                    return new PortugueseStemmer();
                case "ron":
                    return new RomanianStemmer();
                case "rus":
                    return new RussianStemmer();
                case "srp":
                    return new SerbianStemmer();
                case "spa":
                    return new SpanishStemmer();
                case "swe":
                    return new SwedishStemmer();
                case "tam":
                    return new TamilStemmer();
                case "tur":
                    return new TurkishStemmer();
                case "yid":
                    return new YiddishStemmer();
                case "none":
                    return new NoStemmer();
                default:
                    return null;

            }
        }
    }
}
