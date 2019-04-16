using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System;

namespace PhoneScrubber
{
    static public class FileWriter
    {
        /// <summary>
        /// Write out a Do No Call (DNC) formatted list of phone numbers.
        /// </summary>
        /// <param name="output">IEnumerable<ScrubbedOutput></param>
        /// <param name="filePath">full path</param>
        static public void WriteFile(IEnumerable<ScrubbedOutput> output, string filePath)
        {

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer))
            {
                csv.WriteRecords(output);
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Locate your scrubbed phone list here:  ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(filePath);
            Console.ResetColor();
        }
    }
}