using System;
using UniversityClassAutomation.Tests;

namespace UniversityClassAutomation
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine($"Running test at {DateTime.Now}");
                var test = new ClassAutomationTest();
                test.SetUp();
                test.AutomateClassSearch();
                test.TearDown();
                Console.WriteLine("Test completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error running test: {ex.Message}");
            }
        }
    }
}
