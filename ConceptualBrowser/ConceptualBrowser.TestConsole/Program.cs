using ConceptualBrowser.Business.Test;
using System;

namespace ConceptualBrowser.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ConceptExtractionTest.RunTest();

            Console.WriteLine();
            Console.WriteLine("Test completed.");
        }
    }
}