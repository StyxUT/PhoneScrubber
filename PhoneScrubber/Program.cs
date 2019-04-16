using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;

namespace PhoneScrubber
{
    class Program
    {

        static int phoneCount = 0;
        static readonly List<ScrubbedOutput> Output = new List<ScrubbedOutput>();

        static void Main(string[] args)
        {

            // exit immediately if other than 1 parameter is provided
            if (args.Length != 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You must provide a .csv file, and the file name must not contain spaces.");
                Console.ResetColor();
                return;
            }

            // track processing time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            string path = args[0];
            string outputFile = Path.GetDirectoryName(path) + "\\" + Path.GetFileNameWithoutExtension(path) + "-scrubbed.csv";

            // load the phone records
            Console.WriteLine(@"Loading file:  " + path);
            using (var reader = new StreamReader(@path))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;

                var records = csv.GetRecords<DNCScrub>();

                // send records for processing
                ProcessRecords(records);
            }

            // write out results
            FileWriter.WriteFile(Output, outputFile);

            stopwatch.Stop();
            TimeSpan timeSpan = stopwatch.Elapsed;

            // write processing during to console
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(String.Format("Processed {0:N0} phone numbers in {1:ss\\.ffff} seconds.", phoneCount, timeSpan));
            Console.ResetColor();

        }

        /// <summary>
        /// Process phone records in parallel
        /// </summary>
        /// <param name="records">IEnumerable<DNCScrub></param>
        private static void ProcessRecords(IEnumerable<DNCScrub> records)
        {

            Parallel.ForEach(records, new ParallelOptions { MaxDegreeOfParallelism = 1/*int.MaxValue*/ }, record =>
            {
                if (Thread.CurrentThread.Name == null)
                    Thread.CurrentThread.Name = Thread.CurrentThread.ManagedThreadId.ToString();

                Parser parser = new Parser();
                parser.ParseRecord(record);

                ProcessOutput(record);

            });
        }

        /// <summary>
        /// Print phone record to console and add results to Output collection
        /// </summary>
        /// <param name="record">instance of a DNCScrub </param>
        private static void ProcessOutput(DNCScrub record)
        {

            if (!string.IsNullOrEmpty(record.BusinessPhone))
            {
                Console.WriteLine(record.BusinessPhone + "  ->  " + record.ScrubbedBusinessPhone);
                Output.Add(new ScrubbedOutput(record.CaseSafeID, record.BusinessPhone, record.ScrubbedBusinessPhone));
                phoneCount++;
            }

            if (!string.IsNullOrEmpty(record.RegistrationPhone))
            {
                Console.WriteLine(record.RegistrationPhone + "  ->  " + record.ScrubbedRegistrationPhone);
                Output.Add(new ScrubbedOutput(record.CaseSafeID, record.RegistrationPhone, record.ScrubbedRegistrationPhone));
                phoneCount++;
            }

            if (!string.IsNullOrEmpty(record.WidgetPhone))
            {
                Console.WriteLine(record.WidgetPhone + "  ->  " + record.ScrubbedWidgetPhone);
                Output.Add(new ScrubbedOutput(record.CaseSafeID, record.WidgetPhone, record.ScrubbedWidgetPhone));
                phoneCount++;
            }

        }

    }
}
