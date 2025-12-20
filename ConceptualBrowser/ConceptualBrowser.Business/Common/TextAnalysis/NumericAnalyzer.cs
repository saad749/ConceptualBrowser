using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ConceptualBrowser.Business.Common.Stemmer;

namespace ConceptualBrowser.Business.Common.TextAnalysis
{
    /// <summary>
    /// Analyzer for numeric CSV data that implements ITextAnalyzer.
    /// Treats each row as a sentence and each cell value as a token/keyword.
    ///
    /// Like TextAnalyzer for text:
    /// - GetSentences(): splits CSV into rows (each row = one sentence/object)
    /// - Tokenizer(): extracts labeled values from a row (each cell = one token with attr_value format)
    ///
    /// The NumericStemmer rounds values to create equivalence classes,
    /// similar to how text stemmers reduce words to root forms.
    /// </summary>
    public class NumericAnalyzer : ITextAnalyzer
    {
        private readonly NumericStemmer _stemmer;
        private string[] _columnHeaders;
        private int _categoryColumnIndex = -1;

        /// <summary>
        /// Gets or sets whether the first row contains headers.
        /// </summary>
        public bool HasHeaderRow { get; set; } = true;

        /// <summary>
        /// Gets or sets positive category values (case-insensitive).
        /// </summary>
        public HashSet<string> PositiveValues { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "positive", "pos", "1", "true", "yes", "+"
        };

        /// <summary>
        /// Gets the column headers (if HasHeaderRow is true and data has been processed).
        /// </summary>
        public string[] ColumnHeaders => _columnHeaders;

        /// <summary>
        /// Gets the underlying NumericStemmer.
        /// </summary>
        public NumericStemmer Stemmer => _stemmer;

        /// <summary>
        /// Creates a NumericAnalyzer with default precision (-4 = 4 decimal places).
        /// </summary>
        public NumericAnalyzer() : this(-4)
        {
        }

        /// <summary>
        /// Creates a NumericAnalyzer with specified precision.
        /// Negative = decimal places, Zero = integer, Positive = round to 10^n.
        /// </summary>
        /// <param name="precision">Precision value (e.g., -4 for 4 decimal places, +2 for nearest 100)</param>
        public NumericAnalyzer(int precision)
        {
            _stemmer = new NumericStemmer(precision);
        }

        /// <summary>
        /// Splits CSV text into sentences (rows).
        /// Each row becomes one sentence representing an object.
        /// </summary>
        /// <param name="text">CSV content</param>
        /// <returns>List of rows (excluding header if HasHeaderRow is true)</returns>
        public List<string> GetSentences(string text)
        {
            var lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var sentences = new List<string>();

            int startRow = 0;

            // Handle header row
            if (HasHeaderRow && lines.Length > 0)
            {
                var headers = ParseCsvLine(lines[0]);
                _columnHeaders = headers.ToArray();
                _categoryColumnIndex = headers.Count - 1; // Last column is category
                startRow = 1;
            }

            // Each remaining row is a "sentence"
            for (int i = startRow; i < lines.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(lines[i]))
                {
                    sentences.Add(lines[i]);
                }
            }

            return sentences;
        }

        /// <summary>
        /// Same as GetSentences for numeric data (delimiters don't apply).
        /// </summary>
        public List<string> GetSentencesWithDelimiters(string text)
        {
            return GetSentences(text);
        }

        /// <summary>
        /// Tokenizes a CSV row into labeled attribute values.
        /// Each numeric value becomes a token in the format: "attrName_stemmedValue"
        ///
        /// Example row: "1.5432,3.2001,0.8,positive"
        /// With headers: "attr1,attr2,attr3,category"
        /// Tokens: ["attr1_1.5432", "attr2_3.2001", "attr3_0.8000", "[+]"]
        /// </summary>
        /// <param name="sentence">A CSV row</param>
        /// <returns>List of labeled value tokens</returns>
        public List<string> Tokenizer(string sentence)
        {
            var tokens = new List<string>();
            var values = ParseCsvLine(sentence);

            if (values.Count == 0)
                return tokens;

            // Category handling disabled - treat all columns as data - Enabled Now
            int catIndex = _categoryColumnIndex >= 0 ? _categoryColumnIndex : values.Count - 1;

            // Add category marker as a token
            if (catIndex < values.Count)
            {
                string category = values[catIndex].Trim();
                bool isPositive = PositiveValues.Contains(category);
                //tokens.Add(isPositive ? "[+]" : "[-]"); -- No need to add category token as of now
            }

            // Process each attribute value (all columns now treated as data) -- Enabled Now
            int attrIndex = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (i == catIndex)
                    continue; // Skip category column

                string value = values[i].Trim();
                if (string.IsNullOrWhiteSpace(value))
                {
                    attrIndex++;
                    continue;
                }

                string attrName = GetAttributeName(attrIndex);

                // Try to parse as number and stem it
                if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double numValue))
                {
                    string stemmedValue = _stemmer.StemNumeric(numValue);
                    tokens.Add($"{attrName}_{stemmedValue}");
                }
                else
                {
                    // Non-numeric value - include as-is with attribute label
                    tokens.Add($"{attrName}_{value}");
                }

                attrIndex++;
            }

            return tokens;
        }

        /// <summary>
        /// Gets the attribute name for a given index.
        /// Uses column headers if available, otherwise "attr{index}".
        /// </summary>
        private string GetAttributeName(int index)
        {
            // Skip category column when mapping index to headers
            int headerIndex = index;
            if (_categoryColumnIndex >= 0 && _categoryColumnIndex <= index)
            {
                // The category column shifts header indices
                // But we process attributes in order, skipping category
            }

            if (_columnHeaders != null && headerIndex < _columnHeaders.Length)
            {
                // Clean the header name for use as attribute
                string header = _columnHeaders[headerIndex].Trim();
                // Replace spaces with underscores, remove special chars
                header = System.Text.RegularExpressions.Regex.Replace(header, @"[^\w]", "_");
                if (!string.IsNullOrWhiteSpace(header))
                    return header;
            }
            return $"attr{index}";
        }

        /// <summary>
        /// Parses a CSV line, handling quoted values.
        /// </summary>
        private List<string> ParseCsvLine(string line)
        {
            var values = new List<string>();
            bool inQuotes = false;
            var currentValue = new System.Text.StringBuilder();

            foreach (char c in line)
            {
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    values.Add(currentValue.ToString());
                    currentValue.Clear();
                }
                else
                {
                    currentValue.Append(c);
                }
            }
            values.Add(currentValue.ToString());

            return values;
        }

        /// <summary>
        /// Stems a numeric value using the configured precision.
        /// </summary>
        public string Stem(double value)
        {
            return _stemmer.StemNumeric(value);
        }

        /// <summary>
        /// Stems a string value. If it's numeric, rounds to configured precision.
        /// If not numeric, returns as-is (for labeled values like "attr1_1.5432").
        /// Implements ITextAnalyzer.Stem for compatibility with BinaryRelation.
        /// </summary>
        public string Stem(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return word;

            // If it's already a labeled value (e.g., "attr1_1.5432"), return as-is
            // The tokenizer already applies stemming when creating labeled values
            return word;
        }
    }
}
