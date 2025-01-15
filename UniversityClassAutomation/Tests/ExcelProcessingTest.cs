using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using UniversityClassAutomation.Utilities;

namespace UniversityClassAutomation.Tests
{
    [TestFixture]
    public class ExcelProcessingTest
    {
        [Test]
        public void TestExcelFileProcessing()
        {
            var downloadPath = ConfigReader.GetValue("FilePaths", "DownloadPath");
            string latestFile = GetLatestDownloadedFile(downloadPath);

            Assert.IsNotNull(latestFile, "No Excel file found in the Downloads folder.");
            Console.WriteLine($"Processing file: {latestFile}");

            ExcelUtility excelUtility = new ExcelUtility();
            var openClasses = excelUtility.GetOpenClasses(latestFile);

            foreach (var openClass in openClasses)
            {
                Console.WriteLine($"Open Class: {openClass.ClassTitle}, Available Seats: {openClass.AvailableSeats}");
            }
        }

        private string GetLatestDownloadedFile(string directory)
        {
            var files = Directory.GetFiles(directory, "Output_bxp351_*.xlsx")
                                 .Select(f => new FileInfo(f))
                                 .OrderByDescending(f => f.LastWriteTime)
                                 .FirstOrDefault();

            return files?.FullName;
        }
    }
}
