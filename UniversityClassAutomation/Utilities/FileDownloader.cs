using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace UniversityClassAutomation.Utilities
{
    public static class FileDownloader
    {
        public static string WaitForDynamicFile(string directory, string username, int timeoutInSeconds = 30)
        {
            string filePattern = $"Output_{username}_*.xlsx"; // Pattern matching the filename
            int waitTime = timeoutInSeconds;

            while (waitTime > 0)
            {
                var files = Directory.GetFiles(directory, filePattern);
                if (files.Length > 0)
                {
                    return files.OrderByDescending(f => File.GetCreationTime(f)).First(); // Get the latest file
                }

                Thread.Sleep(1000);
                waitTime--;
            }

            throw new FileNotFoundException($"No file matching the pattern {filePattern} was downloaded within the timeout period.");
        }
    }
}
