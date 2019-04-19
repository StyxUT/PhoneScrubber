using CsvHelper.Configuration.Attributes;

namespace PhoneScrubber
{
  public class DNCScrub
  {
    [Index(0)]
    public string CaseSafeID { get; set; }

    [Index(1)]
    public string BusinessPhone { get; set; }

    [Index(2)]
    public string RegistrationPhone { get; set; }

    [Index(3)]
    public string WidgetPhone { get; set; }
  }
}
