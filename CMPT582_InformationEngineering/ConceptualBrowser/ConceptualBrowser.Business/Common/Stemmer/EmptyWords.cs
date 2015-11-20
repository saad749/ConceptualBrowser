using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    class EmptyWords : IEmptyWords
    {
        public List<string> EmptyWordRoots = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8",
            "9", "0", ",", "a", "abl", "aboard", "about", "abov", "accord","el","de",
            "across", "act", "af", "aft", "after","afterward", "again", "against","ago","al",
            "all", "allow", "almost", "alon", "along", "alongsid", "already",
            "also", "although", "alway", "am", "among", "amongst", "an", "and",
            "anoth", "ant","an", "any", "anybody", "anyhow", "anym", "anyon",
            "anyplac", "anyth","anything", "anyway", "anywh","anywhat","anywhere", "apart", "appear",
            "appropry", "are", "ar", "around", "as", "asid","aside", "ask", "assocy",
            "at", "avail", "away", "b", "bar", "be", "becam", "becaus","because",
            "becom", "been", "bef", "behind", "being", "believ", "below",
            "benea", "besid", "best", "bet", "between", "beyond", "bil",
            "both","br", "brief", "but", "by", "c", "cam", "can", "cannot", "cant",
            "caus", "certain", "chang", "cle", "com", "concern", "consequ",
            "consid", "contain", "contr", "correspond", "could", "couldnt",
            "cours", "cur", "d", "definit", "describ", "despit", "did",
            "didnt", "diff", "doe", "doesnt", "doing", "don", "dont", "doubl",
            "down", "downward", "dur", "e", "each", "eg", "eight", "eighteen",
            "eigh", "eighty", "eith", "elev", "els", "elsewh", "enough",
            "entir", "espec", "ev", "everm", "every", "everybody", "everyon",
            "everyplac", "everyth", "everyway", "everywh", "exact", "exampl","even",
            "exceiv", "f", "far", "few", "fewest", "fifteen", "fif", "fifty",
            "first", "fiv", "follow", "for", "forty", "four", "fourteen",
            "from", "furth", "furtherm", "g", "get", "giv", "go", "goe",
            "going", "gon", "got", "greet", "h","ha", "had", "hadnt", "half", "hap",
            "hard", "has", "hasnt", "hav","have" ,"he", "hello", "help", "hent",
            "her", "herself", "hi", "him", "himself", "his", "hop", "how",
            "howev", "hundr", "i", "i'd", "if", "iff", "ign", "i'm", "immedy",
            "in", "indee", "ind", "in", "insid", "insomuch", "instead", "into",
            "is", "isnt", "it", "its", "itself", "iv", "j", "just", "k",
            "keep", "kept", "known", "know", "l", "last", "lat", "least",
            "less", "lest", "let", "lik", "littl", "m", "main", "many", "may",
            "mayb", "me", "mean", "meanwhil", "mer", "might", "mightnt", "mil",
            "min", "modulo", "mor","more", "moreov", "most", "much", "must", "mustnt",
            "my", "myself", "n", "nam", "near", "necess", "need", "neednt",
            "need", "neith", "nev", "nevertheless", "new", "next", "nin",
            "nineteen", "no", "nobody", "non", "nor", "norm", "not", "noth",
            "now", "nowaday", "nowh", "o", "obvy", "of", "off", "oft", "ok",
            "old", "on", "ont", "one", "onto", "opposit", "or", "oth","other","others",
            "otherw", "ought", "our", "out", "outsid", "ov", "overal","over",
            "overmuch", "own", "p", "particul", "past", "pend", "per",
            "perhap", "plac", "pleas", "plu", "poss", "prob", "provid", "q",
            "quit", "r", "rath", "real", "reason", "regard", "regardless",
            "regard", "rel", "respect", "right", "round", "s", "said", "sam",
            "sav", "saw", "say", "second", "see", "seem", "seen", "self",
            "sent", "sery", "seventeen", "seventy", "sev", "shal", "she",
            "should", "shouldnt", "sint","since", "sir","six", "sixteen", "sixty", "so",
            "som", "somebody", "someday", "somehow", "someon", "someplac",
            "someth", "sometim", "somewh", "soon", "sorry", "spec", "stil",
            "sub", "such", "sur", "t", "tak", "tel", "ten", "tend", "than",
            "thank", "that", "the", "thei","they","their", "them", "themselv", "then",
            "ther","there", "thereabout", "thereaft", "thereby", "theref", "thes","these",
            "thi", "think", "third", "thirteen", "thirty", "thy","to", "thorough",
            "thos","those", "though", "thousand", "through", "thu", "tillto", "togeth",
            "took", "toward", "	try", "tril", "twelf", "twelv", "twenty",
            "twic", "two", "u", "un","up", "und","under", "undernea", "unfortun", "unless",
            "unlik", "until", "upon", "us", "use", "unspecified", "unspecifi", "v", "valu", "vary", "vers",
            "very", "via", "visav", "w","wa", "want", "want", "was", "wasnt", "way",
            "we", "welcom", "wel", "went", "wer","were", "what", "whatev", "when",
            "wher", "where", "whereby", "wherev", "wheth", "which", "whil","while",
            "who", "whoev", "whol", "whom", "who", "whos", "why", "will",
            "wish", "with", "within", "without", "wond", "wont", "wor",
            "would", "wouldnt", "ww", "www","x", "y", "yes", "yet", "you", "your", "yourself",
            "z", "ze", ".", ",", "'", "?", "!", "-", "(", ")", "@", "/", ":",
            "_", ";", "+", "&", "%", "*", "=", "<", ">", "$", "[", "]", "{",
            "}", "~", "^", "#", "|", ":-" };

        public string[] EmptyWordRoots2 = new string[]
        {
            "a", "about", "above", "above", "across", "after", "afterwards", "again", "against", "all", "almost", "alone", "along", "already", "also","although","always","am","among", "amongst", "amoungst", "amount",  "an", "and", "another", "any","anyhow","anyone","anything","anyway", "anywhere", "are", "around", "as",  "at", "back","be","became", "because","become","becomes", "becoming", "been", "before", "beforehand", "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom","but", "by", "call", "can", "cannot", "cant", "co", "con", "could", "couldnt", "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", "eight", "either", "eleven","else", "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front", "full", "further", "get", "give", "go", "had", "has", "hasnt", "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", "herself", "him", "himself", "his", "how", "however", "hundred", "ie", "if", "in", "inc", "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", "latterly", "least", "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of", "off", "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over", "own","part", "per", "perhaps", "please", "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", "these", "they", "thickv", "thin", "third", "this", "those", "though", "three", "through", "throughout", "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was", "we", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves", "the"
        };


        public bool IsEmptyWord(string word)
        {
            return EmptyWordRoots.Contains(word.ToLower()) ? true : false;

        }
    }
}

