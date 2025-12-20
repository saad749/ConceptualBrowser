using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConceptualBrowser.Business.Common.Stemmer;
using ConceptualBrowser.Business.Common.TextAnalysis;
using Iso639;

namespace ConceptualBrowser.Business
{
    public class TextAnalyzer : ITextAnalyzer
    {
        public Language Language { get; set; }
        public IStemmer Stemmer { get; set; }
        public string LanguageCode { get; set; }

        // PERFORMANCE OPTIMIZATION: Static cache for stemming results to avoid repeated computation
        private static readonly Dictionary<string, Dictionary<string, string>> _stemCache =
            new Dictionary<string, Dictionary<string, string>>();
        private static readonly object _stemCacheLock = new object();

        public TextAnalyzer(string languageCode)
        {
            LanguageCode = languageCode;
            Language = Language.FromPart3(languageCode);
            Stemmer = Stemmers.GetStemmer(languageCode);

            // PERFORMANCE OPTIMIZATION: Initialize cache for this language if not exists
            lock (_stemCacheLock)
            {
                if (!_stemCache.ContainsKey(languageCode))
                {
                    _stemCache[languageCode] = new Dictionary<string, string>();
                }
            }
        }

        public List<String> GetSentences(string text)
        {
            char[] delimiters = new char[] { '.', '۔' };
            List<string> sentences = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
            return sentences;
        }

        public List<String> GetSentencesWithDelimiters(string text)
        {
            List<String> sentences = new List<string>();
            char[] delimiters = new char[] { '.', ',', ';', '۔' };

            string pattern = @"(\.|,|;)";

            sentences = Regex.Split(text, pattern).Where(s => !String.IsNullOrWhiteSpace(s)).ToList<String>();

            return sentences;
        }

        public List<string> Tokenizer(string sentence)
        {
            List<string> keywords = new List<string>();
            char[] splitChars =
                new char[] { ',', ' ','\r', '\n', '\t', '©', '-', '<', '>', '/', '\\', '.', '(', ')', '?', '@', '^', '#', '%', '&', '*', '$', '!', ';', ':', '\"', '{', '}', '~', '\'', '[', ']', '“' };

            string[] tokens = TryRemoveStopWords(sentence).Split(splitChars);
            foreach (String word in tokens)
            {
                if (word.Length > 1)
                {
                    String root = Stemmer.Stem(word);
                    if (!String.IsNullOrEmpty(TryRemoveStopWords(root)))
                        keywords.Add(word); //Should we add word or root?
                }
            }
            return keywords;
        }

        public string TryRemoveStopWords(string text)
        {
            try
            {
                return text.ToLower(Language.Culture).RemoveStopWords(Language.Part3);
            }
            catch (ArgumentException) //The Language is not supported exception
            {
                return text;
            }
        }

        public string Stem(string word)
        {
            // PERFORMANCE OPTIMIZATION: Use cache to avoid repeated stemming of same words
            var languageCache = _stemCache[LanguageCode];

            if (languageCache.TryGetValue(word, out string cachedStem))
            {
                return cachedStem;
            }

            // Word not in cache, perform stemming and cache the result
            string stem = Stemmer.Stem(word);

            lock (_stemCacheLock)
            {
                // Double-check locking pattern to avoid race conditions
                if (!languageCache.ContainsKey(word))
                {
                    languageCache[word] = stem;
                }
            }

            return stem;
        }

        public string RemoveDiacritics(string InputStr)
        {
            string BasicStr = InputStr.Normalize(NormalizationForm.FormD);
            string TempStr = "";
            for (int i = 0; i < BasicStr.Length; i++)
            {
                if (char.GetUnicodeCategory(BasicStr[i]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    TempStr += BasicStr[i];
            }
            return TempStr;
        }

        public string RemoveStopWords(string text)
        {
            return text.RemoveStopWords(Language.Part3);
        }
    }
}
