using PhoneScrubber;
using System;
using CsvHelper;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("You must provide a file, and the file name must not contain spaces.");
                return;
            }

            Console.WriteLine(@"Loading file:  " + args[0]);
            using (var reader = new StreamReader(@args[0]))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.HeaderValidated = null;
                csv.Configuration.MissingFieldFound = null;

                var records = csv.GetRecords<DNCScrub>();

                Parallel.ForEach(records, new ParallelOptions { MaxDegreeOfParallelism = int.MaxValue }, record =>
                  {
                      if (Thread.CurrentThread.Name == null)
                      {
                          Thread.CurrentThread.Name = Thread.CurrentThread.ManagedThreadId.ToString();
                      }
                      //Console.WriteLine("Start Thread: " + Thread.CurrentThread.Name + "; " + record.CaseSafeID);

                      Parser parser = new Parser();
                      parser.ParseRecord(record);

                      if (record.BusinessPhone != null && record.BusinessPhone.Length > 0)
                          Console.WriteLine(record.BusinessPhone + "  :  " + record.ScrubbedBusinessPhone);

                      if (record.RegistrationPhone != null && record.RegistrationPhone.Length > 0)
                          Console.WriteLine(record.RegistrationPhone + "  :  " + record.ScrubbedRegistrationPhone);

                      if (record.WidgetPhone != null && record.WidgetPhone.Length > 0)
                          Console.WriteLine(record.WidgetPhone + "  :  " + record.ScrubbedWidgetPhone);
                      //Console.WriteLine("End Thread: " + Thread.CurrentThread.Name + "; " + record.CaseSafeID);
                  });
            }
        }

    }
}
