using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace PhoneScrubber
{
    public class Parser
    {

        public DNCScrub ParseRecord(DNCScrub record)
        {
            if (record.BusinessPhone != "")
            {
                Disposition dBusinessPhone = new Disposition(record.BusinessPhone);
                Console.WriteLine(Thread.CurrentThread.Name + " - " + record.CaseSafeID + " (BusinessPhone):  " + dBusinessPhone.Phone + ", " + dBusinessPhone.Plus1Phone + ", " + dBusinessPhone.HasExtension + ", " + dBusinessPhone.ValidPhone + ", " + dBusinessPhone.DNCValidPhone);
                record.ScrubbedBusinessPhone = ParsePhone(dBusinessPhone);
            }

            if (record.WidgetPhone != "")
            {
                Disposition dWidgetPhone = new Disposition(record.WidgetPhone);
                Console.WriteLine(Thread.CurrentThread.Name + " - " + record.CaseSafeID + " (pdWidgetPhone):  " + dWidgetPhone.Phone + ", " + dWidgetPhone.Plus1Phone + ", " + dWidgetPhone.HasExtension + ", " + dWidgetPhone.ValidPhone + ", " + dWidgetPhone.DNCValidPhone);
                record.WidgetPhone = ParsePhone(dWidgetPhone);
            }

            if (record.RegistrationPhone != "")
            {
                Disposition dRegistrationPhone = new Disposition(record.RegistrationPhone);
                Console.WriteLine(Thread.CurrentThread.Name + " - " + record.CaseSafeID + " (RegistrationPhone):  " + dRegistrationPhone.Phone + ", " + dRegistrationPhone.Plus1Phone + ", " + dRegistrationPhone.HasExtension + ", " + dRegistrationPhone.ValidPhone + ", " + dRegistrationPhone.DNCValidPhone);
                record.RegistrationPhone = ParsePhone(dRegistrationPhone);
            }

            return record;
        }

        public string ParsePhone(Disposition disposition)
        {
            while (!disposition.DNCValidPhone && !disposition.CannotBeParsed)
            {
                // don't bother to parse if there are less than 10 digits
                if (DigitsOnly(disposition.Phone).Length < 10)
                {
                    disposition.CannotBeParsed = true;
                }
                // parse a +1.########## formatted phone number
                else if (disposition.Plus1Phone)
                {
                    ScrubPlus1Phone(disposition);
                }
                // attempt to parse out the extension from a phone number
                else if (disposition.HasExtension)
                {
                    ScrubExtension(disposition);

                    string scrubbedPhone = DigitsOnly(disposition.Phone);
                    if (disposition.IsValidPhone(scrubbedPhone))
                    {
                        disposition.Phone = scrubbedPhone;
                    }
                }
                // attempt to parse a generally valid phone number
                else if (disposition.ValidPhone)
                {
                    string scrubbedPhone = DigitsOnly(disposition.Phone);
                    scrubbedPhone = scrubbedPhone.Substring(scrubbedPhone.Length - 10, 10);

                    if (disposition.IsValidPhone(scrubbedPhone))
                    {
                        disposition.Phone = scrubbedPhone;
                    }
                }
                // consider the phone number unparsable
                else
                {
                    string scrubbedPhone = DigitsOnly(disposition.Phone);
                    scrubbedPhone = scrubbedPhone.Substring(scrubbedPhone.Length - 10, 10);

                    if (disposition.IsValidPhone(scrubbedPhone))
                    {
                        disposition.Phone = scrubbedPhone;
                    }
                    else
                    {
                        disposition.CannotBeParsed = true;
                    }
                }

                disposition.DetermineDisposition();
            }

            return disposition.Phone;
        }

        /// <summary>
        /// Scrubs a +1.########## formatted phone number.
        /// </summary>
        /// <param name="d">Disposition</param>
        public void ScrubPlus1Phone(Disposition d)
        {
            try
            {
                d.Phone = d.Phone.Substring(3, 10);
                d.Phone = DigitsOnly(d.Phone);
            }
            catch (Exception ex)
            {
                Console.WriteLine(d.Phone + " - Error Received: " + ex.Message);
                d.CannotBeParsed = true;
            }
        }

        /// <summary>
        /// Attempts to parse out the extension portion of a phone number.
        /// </summary>
        /// <param name="d">Disposition.</param>
        public void ScrubExtension(Disposition d)
        {
            try
            {
                string[] phoneParts = d.Phone.Split(Disposition.extensionIndicators, StringSplitOptions.None);

                d.Phone = phoneParts[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(d.Phone + " - Error Received: " + ex.Message);
                d.CannotBeParsed = true;
            }
        }

        //todo consider making extension method
        /// <summary>
        /// Removes any non-integers.
        /// </summary>
        /// <returns>Phone number stripped of any non-integers</returns>
        /// <param name="phone">Phone number.</param>
        public string DigitsOnly(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");

            return digitsOnly.Replace(phone, "");
        }

    }

}
