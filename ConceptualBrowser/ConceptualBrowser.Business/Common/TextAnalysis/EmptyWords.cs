/* 
    Stopwords/Empty Words are extracted from two main sources:
    https://github.com/stopwords-iso
    https://github.com/Alir3z4/stop-words/tree/162b39dfbda9393dd6ab63d2b8732cef02ab9098
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ConceptualBrowser.Business.Common.TextAnalysis
{
    internal static class EmptyWords
    {
        public static string RemoveStopWords(this string text, string language)
        {
            var emptyWords = LoadStopWords(language);

            text = text.Split(' ').Where(x => !emptyWords.Contains(x)).DefaultIfEmpty().Aggregate((current, next) => current + " " + next);

            return text ?? string.Empty;
        }

        private static List<string> LoadStopWords(string lang)
        {
            var resourceName = string.Format("ConceptualBrowser.Business.Common.TextAnalysis.EmptyWords.{0}.txt", lang);
            var data = LoadData(resourceName);

            var result = data.Split(new[] { "\r\n", "\r", "\n" },
                       StringSplitOptions.None);
            return result.Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        private static string LoadData(string resourceName)
        {
            string result = string.Empty;
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }


    }
}
