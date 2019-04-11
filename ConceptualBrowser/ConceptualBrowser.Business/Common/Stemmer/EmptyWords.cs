using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    public class EmptyWords : IEmptyWords
    {
        public string Language { get; set; }
        public List<string> EmptyWordRoots = new List<string>();
        public string EmptyWordsFilePath { get; set; }
        public string FileName { get; set; }
        public IStemmer Stemmer { get; set; }
        private Dictionary<string, Tuple<string, Encoding>> LanguageFileDictionary { get; set; }
        private Encoding Encoding;

        public EmptyWords(string language, string emptyWordsFilePath = "Assets/", string fileName = "" )
        {
            Language = language;
            EmptyWordsFilePath = emptyWordsFilePath;
            FileName = fileName;
            Initialize();
        }

        #region WordLists
        //EmptyWordRoots = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8",
        //    "9", "0", ",", "a", "abl", "aboard", "about", "abov", "accord","el","de",
        //    "across", "act", "af", "aft", "after","afterward", "again", "against","ago","al",
        //    "all", "allow", "almost", "alon", "along", "alongsid", "already",
        //    "also", "although", "alway", "am", "among", "amongst", "an", "and",
        //    "anoth", "ant","an", "any", "anybody", "anyhow", "anym", "anyon",
        //    "anyplac", "anyth","anything", "anyway", "anywh","anywhat","anywhere", "apart", "appear",
        //    "appropry", "are", "ar", "around", "as", "asid","aside", "ask", "assocy",
        //    "at", "avail", "away", "b", "bar", "be", "becam", "becaus","because",
        //    "becom", "been", "bef", "behind", "being", "believ", "below",
        //    "benea", "besid", "best", "bet", "between", "beyond", "bil",
        //    "both","br", "brief", "but", "by", "c", "cam", "can", "cannot", "cant",
        //    "caus", "certain", "chang", "cle", "com", "concern", "consequ",
        //    "consid", "contain", "contr", "correspond", "could", "couldnt",
        //    "cours", "cur", "d", "definit", "describ", "despit", "did",
        //    "didnt", "diff", "doe", "doesnt", "doing", "don", "dont", "doubl",
        //    "down", "downward", "dur", "e", "each", "eg", "eight", "eighteen",
        //    "eigh", "eighty", "eith", "elev", "els", "elsewh", "enough",
        //    "entir", "espec", "ev", "everm", "every", "everybody", "everyon",
        //    "everyplac", "everyth", "everyway", "everywh", "exact", "exampl","even",
        //    "exceiv", "f", "far", "few", "fewest", "fifteen", "fif", "fifty",
        //    "first", "fiv", "follow", "for", "forty", "four", "fourteen",
        //    "from", "furth", "furtherm", "g", "get", "giv", "go", "goe",
        //    "going", "gon", "got", "greet", "h","ha", "had", "hadnt", "half", "hap",
        //    "hard", "has", "hasnt", "hav","have" ,"he", "hello", "help", "hent",
        //    "her", "herself", "hi", "him", "himself", "his", "hop", "how",
        //    "howev", "hundr", "i", "i'd", "if", "iff", "ign", "i'm", "immedy",
        //    "in", "indee", "ind", "in", "insid", "insomuch", "instead", "into",
        //    "is", "isnt", "it", "its", "itself", "iv", "j", "just", "k",
        //    "keep", "kept", "known", "know", "l", "last", "lat", "least",
        //    "less", "lest", "let", "lik", "littl", "m", "main", "many", "may",
        //    "mayb", "me", "mean", "meanwhil", "mer", "might", "mightnt", "mil",
        //    "min", "modulo", "mor","more", "moreov", "most", "much", "must", "mustnt",
        //    "my", "myself", "n", "nam", "near", "necess", "need", "neednt",
        //    "need", "neith", "nev", "nevertheless", "new", "next", "nin",
        //    "nineteen", "no", "nobody", "non", "nor", "norm", "not", "noth",
        //    "now", "nowaday", "nowh", "o", "obvy", "of", "off", "oft", "ok",
        //    "old", "on", "ont", "one", "onto", "opposit", "or", "oth","other","others",
        //    "otherw", "ought", "our", "out", "outsid", "ov", "overal","over",
        //    "overmuch", "own", "p", "particul", "past", "pend", "per",
        //    "perhap", "plac", "pleas", "plu", "poss", "prob", "provid", "q",
        //    "quit", "r", "rath", "real", "reason", "regard", "regardless",
        //    "regard", "rel", "respect", "right", "round", "s", "said", "sam",
        //    "sav", "saw", "say", "second", "see", "seem", "seen", "self",
        //    "sent", "sery", "seventeen", "seventy", "sev", "shal", "she",
        //    "should", "shouldnt", "sint","since", "sir","six", "sixteen", "sixty", "so",
        //    "som", "somebody", "someday", "somehow", "someon", "someplac",
        //    "someth", "sometim", "somewh", "soon", "sorry", "spec", "stil",
        //    "sub", "such", "sur", "t", "tak", "tel", "ten", "tend", "than",
        //    "thank", "that", "the", "thei","they","their", "them", "themselv", "then",
        //    "ther","there", "thereabout", "thereaft", "thereby", "theref", "thes","these",
        //    "thi", "think", "third", "thirteen", "thirty", "thy","to", "thorough",
        //    "thos","those", "though", "thousand", "through", "thu", "tillto", "togeth",
        //    "took", "toward", "	try", "tril", "twelf", "twelv", "twenty",
        //    "twic", "two", "u", "un","up", "und","under", "undernea", "unfortun", "unless",
        //    "unlik", "until", "upon", "us", "use", "unspecified", "unspecifi", "v", "valu", "vary", "vers",
        //    "very", "via", "visav", "w","wa", "want", "want", "was", "wasnt", "way",
        //    "we", "welcom", "wel", "went", "wer","were", "what", "whatev", "when",
        //    "wher", "where", "whereby", "wherev", "wheth", "which", "whil","while",
        //    "who", "whoev", "whol", "whom", "who", "whos", "why", "will",
        //    "wish", "with", "within", "without", "wond", "wont", "wor",
        //    "would", "wouldnt", "ww", "www","x", "y", "yes", "yet", "you", "your", "yourself",
        //    "z", "ze", ".", ",", "'", "?", "!", "-", "(", ")", "@", "/", ":",
        //    "_", ";", "+", "&", "%", "*", "=", "<", ">", "$", "[", "]", "{",
        //    "}", "~", "^", "#", "|", ":-" };

        #endregion wordlists

        private void Initialize()
        {
            if (String.IsNullOrWhiteSpace((FileName)))
            {
                LanguageFileDictionary = new Dictionary<string, Tuple<string, Encoding>>
                {
                    {"eng", new Tuple<string, Encoding> ("EnglishStopWords_637.csv", Encoding.Default)},
                    {"fra", new Tuple<string, Encoding> ("FrenchStopWords_Unique_FullListFromInternet.txt", Encoding.Default)},
                    {"spa", new Tuple<string, Encoding> ("SpanishStopWords_FullListFromInternet.txt", Encoding.Default)},
                    {"ces", new Tuple<string, Encoding> ("CzechStopWords_468_Unicode.txt", Encoding.Unicode)},
                    {"dan", new Tuple<string, Encoding> ("DanishStopWords_FullListFromInternet.csv", Encoding.Default)},
                    {"nld", new Tuple<string, Encoding> ("DutchStopWords_FullListFromInternet.csv", Encoding.Default)},
                    {"fin", new Tuple<string, Encoding> ("FinnishStopWords_Unicode_FullListFromInternet.txt", Encoding.Unicode)},
                    {"deu", new Tuple<string, Encoding> ("GermanStopWords_FullListFromInternet.csv", Encoding.Default)},
                    {"hun", new Tuple<string, Encoding> ("HungarianStopWords_FullListFromInternet.txt", Encoding.Unicode)},
                    {"ita", new Tuple<string, Encoding> ("ItalianStopWords_FullListFromInternet.csv", Encoding.Default)},
                    {"nor", new Tuple<string, Encoding> ("NorwegianStopWords_FullListFromInternet.csv", Encoding.Default)},
                    {"por", new Tuple<string, Encoding> ("PortugueseStopWords_FullListFromInternet.txt", Encoding.Unicode)},
                    {"ron", new Tuple<string, Encoding> ("RomanianStopWords_FullListFromInternet.txt", Encoding.Unicode)},
                    {"rus", new Tuple<string, Encoding> ("RussianStopWords_FullListFromInternet.txt", Encoding.Unicode)},
                    {"arb", new Tuple<string, Encoding> ("arabic-stop-words-750.txt", Encoding.Unicode)},
                    {"none", new Tuple<string, Encoding> (null, Encoding.Unicode)}
                };

                try
                {
                    FileName = LanguageFileDictionary[Language].Item1;
                    Encoding = LanguageFileDictionary[Language].Item2;
                }
                catch (KeyNotFoundException)
                {
                    throw;
                }
            }

            LoadEmptyWordsFromFile();

            Stemmer = Stemmers.GetStemmer(Language);
        }

        public void LoadEmptyWordsFromFile()
        {
            if (File.Exists(EmptyWordsFilePath))
                EmptyWordRoots = File.ReadAllLines(EmptyWordsFilePath + FileName, Encoding).ToList();
        }

        public void AppendEmptyWordsToFile(string word)
        {
            string root = Stemmer.Stem(word);
            if (File.Exists(EmptyWordsFilePath))
                File.AppendAllText(EmptyWordsFilePath + FileName, Environment.NewLine + root, Encoding);
        }

        public bool IsEmptyWord(string word)
        {
            return EmptyWordRoots.Any( w => String.Equals(word, w, StringComparison.OrdinalIgnoreCase));
        }
    }
}

