using System;
using System.Globalization;

namespace ConceptualBrowser.Business.Common.Stemmer
{
    /// <summary>
    /// A stemmer for numeric data that reduces floating-point values to equivalence classes
    /// by rounding to a specified precision.
    ///
    /// Just like text stemmers reduce words to their root form (e.g., "running" → "run"),
    /// NumericStemmer reduces numbers to their equivalence class (e.g., 1.23456 → 1.2346).
    ///
    /// Precision parameter controls sensitivity with bidirectional support:
    ///
    /// NEGATIVE values = decimal places (finer precision):
    /// - -1: 1 decimal place (0.1 precision) e.g., 1.234 → 1.2
    /// - -2: 2 decimal places (0.01 precision) e.g., 1.234 → 1.23
    /// - -4: 4 decimal places (0.0001 precision, default) e.g., 1.23456 → 1.2346
    /// - -10: 10 decimal places (very fine)
    ///
    /// POSITIVE values = round to nearest power of 10 (coarser precision):
    /// - +1: round to nearest 10 e.g., 1234 → 1230
    /// - +2: round to nearest 100 e.g., 1234 → 1200
    /// - +3: round to nearest 1000 e.g., 1234 → 1000
    ///
    /// ZERO = round to integer:
    /// - 0: round to nearest integer e.g., 1.5 → 2
    /// </summary>
    public class NumericStemmer : IStemmer
    {
        /// <summary>
        /// Precision value controlling rounding behavior.
        /// Negative = decimal places, Zero = integer, Positive = power of 10.
        /// Default is -4 (4 decimal places).
        /// </summary>
        public int Precision { get; set; } = -4;

        /// <summary>
        /// Creates a NumericStemmer with default precision (-4 = 4 decimal places).
        /// </summary>
        public NumericStemmer()
        {
        }

        /// <summary>
        /// Creates a NumericStemmer with specified precision.
        /// </summary>
        /// <param name="precision">Negative for decimal places, positive for rounding to 10^n</param>
        public NumericStemmer(int precision)
        {
            Precision = precision;
        }

        /// <summary>
        /// Stems a string by attempting to parse it as a number and rounding.
        /// If the string is not a valid number, returns it unchanged.
        /// </summary>
        /// <param name="s">Input string (may be a number or text)</param>
        /// <returns>Rounded number as string, or original string if not numeric</returns>
        public string Stem(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return s;

            // Try to parse as a number
            if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                return StemNumeric(value);
            }

            // Not a number, return as-is
            return s;
        }

        /// <summary>
        /// Stems a numeric value by rounding to the specified precision.
        /// </summary>
        /// <param name="value">The numeric value to stem</param>
        /// <returns>Rounded value as string</returns>
        public string StemNumeric(double value)
        {
            double rounded;
            string format;

            if (Precision < 0)
            {
                // Negative precision = decimal places
                int decimalPlaces = -Precision;
                rounded = Math.Round(value, decimalPlaces, MidpointRounding.AwayFromZero);
                format = $"F{decimalPlaces}";
            }
            else if (Precision == 0)
            {
                // Zero = round to integer
                rounded = Math.Round(value, MidpointRounding.AwayFromZero);
                format = "F0";
            }
            else
            {
                // Positive precision = round to nearest 10^Precision
                double factor = Math.Pow(10, Precision);
                rounded = Math.Round(value / factor, MidpointRounding.AwayFromZero) * factor;
                format = "F0";
            }

            return rounded.ToString(format, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Stems a numeric value and creates a labeled attribute string.
        /// Format: "attr{index}_{rounded_value}"
        /// </summary>
        /// <param name="value">The numeric value to stem</param>
        /// <param name="attributeIndex">The column/attribute index</param>
        /// <returns>Labeled value string (e.g., "attr1_3.1416")</returns>
        public string StemWithLabel(double value, int attributeIndex)
        {
            string stemmed = StemNumeric(value);
            return $"attr{attributeIndex}_{stemmed}";
        }

        /// <summary>
        /// Gets the effective DELTA value for this stemmer's precision.
        /// Two values are equivalent if they round to the same stemmed value.
        /// </summary>
        public double EffectiveDelta
        {
            get
            {
                // The effective delta is half of the smallest representable difference
                // For negative precision (decimal places): 0.5 * 10^precision
                // For positive precision (power of 10): 0.5 * 10^precision
                return 0.5 * Math.Pow(10, Precision);
            }
        }

        /// <summary>
        /// Gets a human-readable description of the precision setting.
        /// </summary>
        public string PrecisionDescription
        {
            get
            {
                if (Precision < 0)
                    return $"{-Precision} decimal places";
                else if (Precision == 0)
                    return "Integer (no decimals)";
                else
                    return $"Round to nearest {Math.Pow(10, Precision):N0}";
            }
        }
    }
}
