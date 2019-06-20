using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ConceptualBrowser.Business.Common.Stemmer;

namespace ConceptualBrowser.Business
{
    public class TextAnalyzer : ITextAnalyzer
    {
        public IStemmer Stemmer { get; set; }
        public IEmptyWords EmptyWords { get; set; }

        public TextAnalyzer(IStemmer stemmer, IEmptyWords emptywords)
        {
            Stemmer = stemmer;
            EmptyWords = emptywords;
        }

        public TextAnalyzer()
        {

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
                new char[] { ',', ' ', '\n', '\t', '©', '-', '<', '>', '/', '\\', '.', '(', ')', '?', '@', '^', '#', '%', '&', '*', '$', '!', ';', ':', '\"', '{', '}', '~', '\'', '[', ']', '“' };
            string[] tokens = sentence.Split(splitChars);
            foreach (String word in tokens)
            {
                if (word.Length > 1)
                {
                    String root = Stemmer.Stem(word);
                    if (!EmptyWords.IsEmptyWord(root) && !EmptyWords.IsEmptyWord(word))
                        keywords.Add(word);
                }
            }
            return keywords;
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
    }
}
