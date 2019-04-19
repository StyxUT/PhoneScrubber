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
      { "Success", 0 },
      { "Error", 0 },
      { "Null or Empty", 0 }
    };
    static readonly List<ScrubbedOutput> Output = new List<ScrubbedOutput>();
    private const string Region = "US";

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

        TryParseNumber(record.CaseSafeID, record.BusinessPhone);
        TryParseNumber(record.CaseSafeID, record.WidgetPhone);
        TryParseNumber(record.CaseSafeID, record.RegistrationPhone);
      });
    }

    private static void TryParseNumber(string id, string value)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        AddResult("Null or Empty");
        return;
      }

      try
      {
        var phoneUtil = PhoneNumbers.PhoneNumberUtil.GetInstance();
        var phone = phoneUtil.Parse(value, Region);
        Output.Add(new ScrubbedOutput(id, value, phone.NationalNumber.ToString()));
        Console.WriteLine($"{value}  ->  {phone.NationalNumber}");
        AddResult("Success");
      }
      catch
      {
        AddResult("Error");
      }
    }

    private static void AddResult(string key)
    {
      Results.TryGetValue(key, out var i);
      Results[key] = i + 1;
    }
  }
}
