using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace PhoneScrubber
{
    static class FileLoader
    {
        /// <summary>
        /// Load formatted file with phone numbers to be scrubbed.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>IEnumerable<DNCScrub> - loaded phone records</returns>
        static public IEnumerable<DNCScrub> LoadFile(string filePath)
        {
            Console.WriteLine(@"Loading file:  " + filePath);
            using (var reader = new StreamReader(@filePath))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;

                var records = csv.GetRecords<DNCScrub>();

                return records;
            }
        }

    }
}