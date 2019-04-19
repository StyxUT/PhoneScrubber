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
    static Dictionary<string, int> Results = new Dictionary<string, int>() {
      { "Success", 0 }
    };
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
      Console.WriteLine($"Loading file: {path}");

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

      Console.ForegroundColor = ConsoleColor.Blue;
      Console.WriteLine($"Processed: {timeSpan:ss\\.ffff} seconds.");
      foreach(var key in Results.Keys)
      {
        Console.WriteLine($"\t{key}: {Results[key]:N0}");
      }
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

        Parser.ParseRecord(record);
        ProccessRecordOutput(record);
      });
    }

    /// <summary>
    /// Print phone record to console and add results to Output collection
    /// </summary>
    /// <param name="record">instance of a DNCScrub </param>
    private static void ProccessRecordOutput(DNCScrub record)
    {
      void AddResult(string key)
      {
        Results.TryGetValue(key, out var i);
        Results[key] = i + 1;
      }

      void Check(string id, string original, Disposition d)
      {
        if (d.CannotBeParsed)
        {
          if (!Results.ContainsKey(d.Value))
            Results.Add(d.Value, 0);
          AddResult(d.Value);
          return;
        }

        Console.WriteLine($"{original}  ->  {d.Value}");
        Output.Add(new ScrubbedOutput(id, original, d.Value));
        AddResult("Success");
      }

      if (record.ScrubbedBusinessPhone.CannotBeParsed)
        Check(record.CaseSafeID, record.BusinessPhone, record.ScrubbedBusinessPhone);

      if (!record.ScrubbedRegistrationPhone.CannotBeParsed)
        Check(record.CaseSafeID, record.RegistrationPhone, record.ScrubbedRegistrationPhone);

      if (!string.IsNullOrEmpty(record.WidgetPhone))
        Check(record.CaseSafeID, record.WidgetPhone, record.ScrubbedWidgetPhone);
    }
  }
}
