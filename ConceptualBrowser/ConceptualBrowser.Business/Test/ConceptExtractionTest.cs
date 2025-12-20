using ConceptualBrowser.Business.Common;
using ConceptualBrowser.Business.Common.Stemmer;
using ConceptualBrowser.Business.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConceptualBrowser.Business.Test
{
    public class ConceptExtractionTest
    {
        public static void RunTest()
        {
            // Run the numeric category test first
            RunNumericCategoryTest();
            Console.WriteLine("\n" + new string('=', 60) + "\n");

            // Then run the original text test
            Console.WriteLine("=== ConceptualBrowser Concept Extraction Test ===");
            Console.WriteLine($"Test Date: {DateTime.Now}");
            Console.WriteLine($"Branch: performance-optimization-phase2 (FIXED)");
            Console.WriteLine();

            // Load the test text
            string testFilePath = Path.Combine("..", "ConceptualBrowser.FormUI", "Assets", "Project Text Samples", "English", "BookChapters.txt");

            if (!File.Exists(testFilePath))
            {
                Console.WriteLine($"ERROR: Test file not found at {testFilePath}");
                return;
            }

            string testText = File.ReadAllText(testFilePath);
            Console.WriteLine($"Test text loaded: {testText.Length} characters");
            Console.WriteLine($"First 100 chars: {testText.Substring(0, Math.Min(100, testText.Length))}...");
            Console.WriteLine();

            // Test parameters
            string languageCode = "eng"; // Use ISO 639-3 three-letter code
            double coveragePercentage = 1; // Increased to 100% coverage to capture more concepts

            try
            {
                Console.WriteLine("Starting concept extraction...");
                var conceptExtraction = new ConceptExtraction();
                var concepts = conceptExtraction.Extract(testText, languageCode, coveragePercentage, null);

                Console.WriteLine($"Extraction completed. Total concepts found: {concepts?.Count ?? 0}");
                Console.WriteLine();

                if (concepts == null || concepts.Count == 0)
                {
                    Console.WriteLine("ERROR: No concepts extracted!");
                    return;
                }

                // Display top 10 concepts
                Console.WriteLine("=== TOP 10 CONCEPTS ===");
                var topConcepts = concepts.OrderByDescending(c => c.Gain).Take(10).ToList();

                for (int i = 0; i < topConcepts.Count; i++)
                {
                    var concept = topConcepts[i];
                    Console.WriteLine($"{i + 1:D2}. Gain: {concept.Gain:F4} | Name: '{concept.ConceptName}' | Sentences: {concept.Sentences?.Count ?? 0}");

                    // Show first few words of sentences for context
                    if (concept.Sentences != null && concept.Sentences.Count > 0)
                    {
                        var firstSentence = concept.Sentences.First().OriginalSentence;
                        var preview = firstSentence.Length > 60 ? firstSentence.Substring(0, 60) + "..." : firstSentence;
                        Console.WriteLine($"    Sample: \"{preview}\"");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("=== DIAGNOSTIC INFORMATION ===");
                Console.WriteLine($"Highest gain: {concepts.Max(c => c.Gain):F4}");
                Console.WriteLine($"Lowest gain: {concepts.Min(c => c.Gain):F4}");
                Console.WriteLine($"Average gain: {concepts.Average(c => c.Gain):F4}");
                Console.WriteLine($"Concepts with gain > 0: {concepts.Count(c => c.Gain > 0)}");
                Console.WriteLine($"Concepts with gain = -1: {concepts.Count(c => c.Gain == -1)}");
                Console.WriteLine($"Concepts with gain = 0: {concepts.Count(c => c.Gain == 0)}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR during concept extraction: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        /// <summary>
        /// Tests the new category feature with numeric CSV data.
        /// </summary>
        public static void RunNumericCategoryTest()
        {
            Console.WriteLine("=== NUMERIC CATEGORY TEST ===");
            Console.WriteLine($"Test Date: {DateTime.Now}");
            Console.WriteLine();

            // Create sample numeric CSV data with category column (last column)
            // Format: attr1, attr2, attr3, category
            string numericCsv = @"temperature,humidity,pressure,category
25.5,60.2,1013.25,positive
26.1,58.5,1012.80,positive
24.8,62.1,1014.00,positive
18.2,75.5,1008.50,negative
17.9,78.2,1007.90,negative
19.1,72.0,1009.10,negative
25.8,59.0,1013.50,positive
18.5,74.0,1008.80,negative
26.0,61.5,1012.90,positive
17.5,80.0,1007.50,negative";

            Console.WriteLine("Sample CSV Data:");
            Console.WriteLine(numericCsv);
            Console.WriteLine();

            try
            {
                // Extract concepts using numeric mode
                var conceptExtraction = new ConceptExtraction();
                var concepts = conceptExtraction.Extract(numericCsv, Stemmers.NumericCode, 1.0, null);

                Console.WriteLine($"Extraction completed. Total concepts found: {concepts?.Count ?? 0}");
                Console.WriteLine();

                if (concepts == null || concepts.Count == 0)
                {
                    Console.WriteLine("ERROR: No concepts extracted!");
                    return;
                }

                // Display concepts with category information
                Console.WriteLine("=== CONCEPTS WITH CATEGORY INFO ===");
                Console.WriteLine($"{"#",-3} {"Gain",-8} {"Category",-10} {"Cat%",-8} {"Pos/Neg",-10} {"Pos%",-8} {"Neg%",-8} {"Name",-20}");
                Console.WriteLine(new string('-', 90));

                var topConcepts = concepts.OrderByDescending(c => c.Gain).Take(15).ToList();
                for (int i = 0; i < topConcepts.Count; i++)
                {
                    var concept = topConcepts[i];
                    string categoryDisplay = concept.Category ?? "N/A";
                    string ratioDisplay = $"{concept.PositiveCount}/{concept.NegativeCount}";

                    Console.WriteLine($"{i + 1,-3} {concept.Gain,-8:F2} {categoryDisplay,-10} {concept.CategoryPercentage,-8:F1} {ratioDisplay,-10} {concept.PositivePercentage,-8:F1} {concept.NegativePercentage,-8:F1} {concept.ConceptName,-20}");
                }

                Console.WriteLine();
                Console.WriteLine("=== CATEGORY SUMMARY ===");
                var conceptsWithCategory = concepts.Where(c => c.Category != null).ToList();
                var positiveConcepts = conceptsWithCategory.Count(c => c.Category == "Positive");
                var negativeConcepts = conceptsWithCategory.Count(c => c.Category == "Negative");
                Console.WriteLine($"Concepts with category data: {conceptsWithCategory.Count}");
                Console.WriteLine($"Positive concepts: {positiveConcepts}");
                Console.WriteLine($"Negative concepts: {negativeConcepts}");

                // Verify sentence categories are populated
                Console.WriteLine();
                Console.WriteLine("=== SENTENCE CATEGORY VERIFICATION ===");
                if (concepts.Count > 0 && concepts[0].Sentences?.Count > 0)
                {
                    var firstConcept = concepts.First(c => c.Sentences?.Count > 0);
                    Console.WriteLine($"First concept '{firstConcept.ConceptName}' sentences:");
                    foreach (var sentence in firstConcept.Sentences.Take(3))
                    {
                        Console.WriteLine($"  - Index: {sentence.SentenceIndex}, Category: {sentence.Category ?? "null"}, IsPositive: {sentence.IsPositive}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR during numeric concept extraction: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }
    }
}