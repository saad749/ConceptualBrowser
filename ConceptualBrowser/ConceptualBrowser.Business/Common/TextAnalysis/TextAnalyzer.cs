using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConceptualBrowser.Business.Common.Stemmer;
using Iso639;
using StopWord;

namespace ConceptualBrowser.Business
{
    public class TextAnalyzer : ITextAnalyzer
    {
        public Language Language { get; set; }
        public IStemmer Stemmer { get; set; }
        //public IEmptyWords EmptyWords { get; set; }

        public TextAnalyzer(string languageCode)
        {
            Language = Language.FromPart3(languageCode);
            Stemmer = Stemmers.GetStemmer(languageCode);
            //EmptyWords = emptywords;
            
        }

        public List<String> GetSentences(string text)
        {
            char[] delimiters = new char[] { '.' };
            List<string> sentences = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList();
            return sentences;
        }

        public List<String> GetSentencesWithDelimiters(string text)
        {
            List<String> sentences = new List<string>();
            char[] delimiters = new char[] { '.', ',', ';' };

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
                return text.RemoveStopWords(Language.Part1);
            }
            catch (ArgumentException ex) //The Language is not supported exception
            {
                return text;
            }
        }

        public string Stem(string word)
        {
            return Stemmer.Stem(word);
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
            return text.RemoveStopWords(Language.Part1);
        }
    }
}
