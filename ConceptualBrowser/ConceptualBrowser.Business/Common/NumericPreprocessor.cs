using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using ConceptualBrowser.Business.Common.Stemmer;

namespace ConceptualBrowser.Business.Common
{
    /// <summary>
    /// Preprocessor that converts numeric matrix data into text format suitable for FCA processing.
    ///
    /// Input: CSV matrix where each row is an object, columns are features,
    ///        and the last column is the category (positive/negative).
    ///
    /// Output: Text where each row becomes a "sentence" with labeled attribute values as "keywords".
    ///
    /// Example:
    /// Input CSV:
    ///   1.5432, 3.2001, positive
    ///   1.5487, 2.1000, negative
    ///
    /// Output Text (with 4 decimal places):
    ///   [positive] attr0_1.5432 attr1_3.2001
    ///   [negative] attr0_1.5487 attr1_2.1000
    /// </summary>
    public class NumericPreprocessor
    {
        private readonly NumericStemmer _stemmer;

        /// <summary>
        /// Gets or sets the column names/headers. If null, uses "attr0", "attr1", etc.
        /// </summary>
        public string[] ColumnHeaders { get; set; }

        /// <summary>
        /// Gets or sets whether the first row of the CSV contains headers.
        /// </summary>
        public bool HasHeaderRow { get; set; } = false;

        /// <summary>
        /// Gets or sets the index of the category column (default: last column, -1).
        /// </summary>
        public int CategoryColumnIndex { get; set; } = -1;

        /// <summary>
        /// Gets or sets positive category values (case-insensitive).
        /// Default: "positive", "pos", "1", "true", "yes", "+"
        /// </summary>
        public HashSet<string> PositiveValues { get; set; } = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "positive", "pos", "1", "true", "yes", "+"
        };

        /// <summary>
        /// Creates a NumericPreprocessor with default precision (-4 = 4 decimal places).
        /// </summary>
        public NumericPreprocessor() : this(-4)
        {
        }

        /// <summary>
        /// Creates a NumericPreprocessor with specified precision.
        /// Negative = decimal places, Zero = integer, Positive = round to 10^n.
        /// </summary>
        /// <param name="precision">Precision value (e.g., -4 for 4 decimal places, +2 for nearest 100)</param>
        public NumericPreprocessor(int precision)
        {
            _stemmer = new NumericStemmer(precision);
        }

        /// <summary>
        /// Gets the underlying NumericStemmer.
        /// </summary>
        public NumericStemmer Stemmer => _stemmer;

        /// <summary>
        /// Converts a CSV string to text format for FCA processing.
        /// </summary>
        /// <param name="csvContent">CSV content with numeric data</param>
        /// <returns>Text suitable for the existing FCA pipeline</returns>
        public string ConvertCsvToText(string csvContent)
        {
            var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return ConvertLinesToText(lines);
        }

        /// <summary>
        /// Converts CSV lines to text format for FCA processing.
        /// </summary>
        /// <param name="lines">Array of CSV lines</param>
        /// <returns>Text suitable for the existing FCA pipeline</returns>
        public string ConvertLinesToText(string[] lines)
        {
            if (lines == null || lines.Length == 0)
                return string.Empty;

            var sb = new StringBuilder();
            int startRow = 0;

            // Handle header row
            if (HasHeaderRow && lines.Length > 0)
            {
                var headers = ParseCsvLine(lines[0]);
                ColumnHeaders = headers.ToArray();
                startRow = 1;
            }

            // Process data rows
            for (int i = startRow; i < lines.Length; i++)
            {
                string processedLine = ProcessRow(lines[i], i - startRow);
                if (!string.IsNullOrWhiteSpace(processedLine))
                {
                    sb.AppendLine(processedLine);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reads a CSV file and converts it to text format for FCA processing.
        /// </summary>
        /// <param name="filePath">Path to the CSV file</param>
        /// <returns>Text suitable for the existing FCA pipeline</returns>
        public string ConvertCsvFileToText(string filePath)
        {
            string csvContent = File.ReadAllText(filePath);
            return ConvertCsvToText(csvContent);
        }

        /// <summary>
        /// Processes a single row of CSV data into a text "sentence".
        /// </summary>
        /// <param name="line">CSV line</param>
        /// <param name="rowIndex">Row index (for object identification)</param>
        /// <returns>Processed text line</returns>
        private string ProcessRow(string line, int rowIndex)
        {
            var values = ParseCsvLine(line);
            if (values.Count == 0)
                return null;

            // Determine category column index
            int catIndex = CategoryColumnIndex >= 0 ? CategoryColumnIndex : values.Count - 1;

            // Get category
            string category = catIndex < values.Count ? values[catIndex].Trim() : "unknown";
            bool isPositive = PositiveValues.Contains(category);
            string categoryMarker = isPositive ? "[+]" : "[-]";

            // Build the sentence with labeled attribute values
            var sb = new StringBuilder();
            sb.Append(categoryMarker);

            int attrIndex = 0;
            for (int i = 0; i < values.Count; i++)
            {
                if (i == catIndex)
                    continue; // Skip category column

                string value = values[i].Trim();
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                // Try to parse as number
                if (double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double numValue))
                {
                    string attrName = GetAttributeName(attrIndex);
                    string stemmedValue = _stemmer.StemNumeric(numValue);
                    sb.Append($" {attrName}_{stemmedValue}");
                }
                else
                {
                    // Non-numeric value - include as-is with attribute label
                    string attrName = GetAttributeName(attrIndex);
                    sb.Append($" {attrName}_{value}");
                }
                attrIndex++;
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the attribute name for a given index.
        /// </summary>
        private string GetAttributeName(int index)
        {
            if (ColumnHeaders != null && index < ColumnHeaders.Length)
            {
                // Clean the header name for use as attribute
                string header = ColumnHeaders[index].Trim().Replace(" ", "_");
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
            var currentValue = new StringBuilder();

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
        /// Gets statistics about the numeric data.
        /// </summary>
        public NumericDataStats GetStats(string csvContent)
        {
            var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var stats = new NumericDataStats();

            int startRow = HasHeaderRow ? 1 : 0;

            foreach (var line in lines.Skip(startRow))
            {
                var values = ParseCsvLine(line);
                int catIndex = CategoryColumnIndex >= 0 ? CategoryColumnIndex : values.Count - 1;

                if (catIndex < values.Count)
                {
                    string category = values[catIndex].Trim();
                    if (PositiveValues.Contains(category))
                        stats.PositiveCount++;
                    else
                        stats.NegativeCount++;
                }
                stats.TotalRows++;
            }

            if (lines.Length > startRow)
            {
                var firstDataLine = ParseCsvLine(lines[startRow]);
                stats.AttributeCount = firstDataLine.Count - 1; // Exclude category column
            }

            return stats;
        }
    }

    /// <summary>
    /// Statistics about numeric data.
    /// </summary>
    public class NumericDataStats
    {
        public int TotalRows { get; set; }
        public int AttributeCount { get; set; }
        public int PositiveCount { get; set; }
        public int NegativeCount { get; set; }
    }
}
