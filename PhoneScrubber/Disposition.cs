using System.Text.RegularExpressions;

namespace PhoneScrubber
{

  public class Disposition
  {
    // list of strings used to determine that a phone number contains an extension.
    public static string[] ExtensionIndicators = { "ext", "ete", "x" };

    public string Phone { get; set; }
    public bool CannotBeParsed { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:PhoneScrubber.Disposition"/> class, and determines the disposition of a phone number.
    /// </summary>
    /// <param name="phone">Phone number.</param>
    public Disposition(string phone)
    {
      this.Phone = phone;
    }

    /// <summary>
    /// Check to see if the phone has a Plus1.
    /// </summary>
    public bool Plus1Phone() => Regex.IsMatch(this.Phone, @"^([+][1][.])([0-9]{10})$");
    /// <summary>
    /// Check to see if the phone has an extension.
    /// </summary>
    public bool HasExtension() => this.Phone.ContainsAny(ExtensionIndicators);
    /// <summary>
    /// Check to see if the phone number is DNC valid.
    /// </summary>
    public bool DNCValidPhone() => Regex.IsMatch(this.Phone, "^[0-9]{10}$");
    /// <summary>
    /// Check to see if the phone number is valid.
    /// </summary>
    public bool ValidPhone() => Regex.IsMatch(this.Phone, @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
  }
}

