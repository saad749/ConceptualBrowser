using ConceptualBrowser.Business.Common;
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
            double coveragePercentage = 0.95; // Increased to 95% coverage to capture more concepts

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
    }
}