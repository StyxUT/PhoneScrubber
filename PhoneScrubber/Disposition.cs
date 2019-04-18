using System.Text.RegularExpressions;

namespace PhoneScrubber
{

  public class Disposition
  {
    // list of strings used to determine that a phone number contains an extension.
    public static readonly string[] extensionIndicators = new string[] { "ext", "ete", "x" };

    public string Phone { get; set; }

    /// <summary>
    /// Does the phone have an extension.
    /// </summary>
    public bool HasExtension() => this.Phone.ContainsAny(extensionIndicators);
    /// <summary>
    /// Dispositions the phone number as DNC valid.
    /// </summary>
    /// <returns><c>true</c>, if a phone number is DNC valie <c>false</c> otherwise.</returns>
    public bool DNCValidPhone() => Regex.IsMatch(this.Phone, "^[0-9]{10}$");
    /// <summary>
    /// Dispositions the phone number as being in a +1.###-###-#### format.
    /// </summary>
    /// <returns><c>true</c>, if a phone number is a +1.###-###-#### format <c>false</c> otherwise.</returns>
    public bool Plus1Phone() => Regex.IsMatch(this.Phone, @"^([+][1][.])([0-9]{10})$");
    public bool ValidPhone() => Regex.IsMatch(this.Phone, @"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
    public bool CannotBeParsed { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:PhoneScrubber.Disposition"/> class, and determines the disposition of a phone number.
    /// </summary>
    /// <param name="phone">Phone number.</param>
    public Disposition(string phone)
    {
      this.Phone = phone;
    }
  }
}

