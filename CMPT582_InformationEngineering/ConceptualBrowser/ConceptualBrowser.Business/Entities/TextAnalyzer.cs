using ConceptualBrowser.Business.Common;
using ConceptualBrowser.Business.Common.Stemmer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConceptualBrowser.Business.Entities
{
    class TextAnalyzer : ITextAnalyzer
    {
        public List<String> GetSentences(string text)
        {
            List<String> sentences = new List<string>();
            char[] delimiters = new char[] { '.', ',', ';' };

            sentences = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).ToList<String>();

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
                new char[] { ',', ' ', '\n', '\t', '©', '-', '<', '>', '/', '\\', '.', '(', ')', '?', '@', '^', '#', '%', '&', '*', '$', '!', ';', ':', '\"', '{', '}', '~', '\'', '[',']', '“' };
            string[] tokens = sentence.Split(splitChars);
            foreach (String word in tokens)
            {
                if(word.Length > 1)
                {
                    IStemmer stemmer = new EnglishStemmer();
                    String root = stemmer.Stem(word);
                    if (!IsEmptyWord(root))
                        keywords.Add(word);
                }
            }
            return keywords;
        }

        public bool IsEmptyWord(string word)
        {
            EmptyWords emptyWords = new EmptyWords();

            return emptyWords.EmptyWordRoots.Contains(word.ToLower()) ? true : false;

        }


    }
}
