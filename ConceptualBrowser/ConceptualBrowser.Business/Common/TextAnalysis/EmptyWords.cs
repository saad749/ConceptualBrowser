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
        // PERFORMANCE OPTIMIZATION: Cache stop words to avoid repeated file loading
        private static readonly Dictionary<string, HashSet<string>> _stopWordCache = new Dictionary<string, HashSet<string>>();
        private static readonly object _cacheLock = new object();

        public static string RemoveStopWords(this string text, string language)
        {
            var emptyWords = GetCachedStopWords(language);

            text = text.Split(' ').Where(x => !emptyWords.Contains(x)).DefaultIfEmpty().Aggregate((current, next) => current + " " + next);

            return text ?? string.Empty;
        }

        /// <summary>
        /// PERFORMANCE OPTIMIZATION: Get stop words from cache or load once and cache
        /// </summary>
        private static HashSet<string> GetCachedStopWords(string language)
        {
            if (_stopWordCache.TryGetValue(language, out var cachedWords))
            {
                return cachedWords;
            }

            lock (_cacheLock)
            {
                // Double-check locking pattern
                if (_stopWordCache.TryGetValue(language, out cachedWords))
                {
                    return cachedWords;
                }

                // Load stop words once and cache as HashSet for O(1) lookups
                var stopWordsList = LoadStopWords(language);
                var stopWordsSet = new HashSet<string>(stopWordsList);
                _stopWordCache[language] = stopWordsSet;
                return stopWordsSet;
            }
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
