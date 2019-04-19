using System;
using System.Text.RegularExpressions;

namespace PhoneScrubber
{
  public static class Parser
  {
    public static DNCScrub ParseRecord(DNCScrub record)
    {
      record.ScrubbedBusinessPhone = new Disposition(record.BusinessPhone);
      ParsePhone(record.ScrubbedBusinessPhone);

      record.ScrubbedWidgetPhone = new Disposition(record.WidgetPhone);
      ParsePhone(record.ScrubbedWidgetPhone);

      record.ScrubbedRegistrationPhone = new Disposition(record.RegistrationPhone);
      ParsePhone(record.ScrubbedRegistrationPhone);

      return record;
    }

    /// <summary>
    /// Parse a phone.
    /// </summary>
    /// <returns>The phone.</returns>
    /// <param name="disposition">Disposition.</param>
    public static string ParsePhone(Disposition disposition)
    {
      if (string.IsNullOrWhiteSpace(disposition.Value))
      {
        disposition.SetValue("Null or Empty");
        return disposition.Value;
      }

      if (disposition.IsEmailAddress())
      {
        disposition.SetValue("Email Address");
        return disposition.Value;
      }

      while (!disposition.DNCValidPhone() && !disposition.CannotBeParsed)
      {
        // don't bother to parse if there are less than 10 digits
        if (DigitsOnly(disposition.Value).Length < 10)
        {
          disposition.SetValue("Too few digits");
        }
        // parse a +1.########## formatted phone number
        else if (disposition.Plus1Phone())
        {
          ScrubPlus1Phone(disposition);
        }
        // attempt to parse out the extension from a phone number
        else if (disposition.HasExtension())
        {
          ScrubExtension(disposition);
        }
        else
        {
          // attempt to parse a generally valid phone number
          string scrubbedPhone = DigitsOnly(disposition.Value);
          scrubbedPhone = scrubbedPhone.Substring(scrubbedPhone.Length - 10, 10);
          disposition.Value = scrubbedPhone;

          if (!disposition.ValidPhone())
          {
            disposition.SetValue("Could not parse.");
          }
        }
      }

      return disposition.Value;
    }

    /// <summary>
    /// Scrubs a +1.########## formatted phone number.
    /// </summary>
    /// <param name="d">Disposition</param>
    public static void ScrubPlus1Phone(Disposition d)
    {
      try
      {
        d.Value = d.Value.Substring(3, 10);
        d.Value = DigitsOnly(d.Value);
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{d.Value} - Error Received: {ex.Message}");
        d.SetValue("Error Received");
      }
    }

    /// <summary>
    /// Attempts to parse out the extension portion of a phone number.
    /// </summary>
    /// <param name="d">Disposition.</param>
    public static void ScrubExtension(Disposition d)
    {
      try
      {
        string[] phoneParts = d.Value.Split(Disposition.ExtensionIndicators, StringSplitOptions.None);
        d.Value = phoneParts[0];
      }
      catch (Exception ex)
      {
        Console.WriteLine($"{d.Value} - Error Received: {ex.Message}");
        d.SetValue("Error Received");
      }
    }

    /// <summary>
    /// Removes any non-integers.
    /// </summary>
    /// <returns>Phone number stripped of any non-integers</returns>
    /// <param name="phone">Phone number.</param>
    public static string DigitsOnly(string phone)
    {
      if (string.IsNullOrWhiteSpace(phone))
        return "";
      return Regex.Replace(phone, @"[^\d]", "");
    }
  }
}
