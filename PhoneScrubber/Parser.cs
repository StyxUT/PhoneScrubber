using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace PhoneScrubber
{
    public class Parser
    {

        public DNCScrub ParseRecord(DNCScrub record)
        {
            if (record.BusinessPhone != "")
            {
                PhoneDisposition pdBusinessPhone = new PhoneDisposition(record.BusinessPhone);
                Console.WriteLine(record.CaseSafeID + " (BusinessPhone):  " + pdBusinessPhone.Phone + ", " + pdBusinessPhone.Plus1Phone + ", " + pdBusinessPhone.HasExtension + ", " + pdBusinessPhone.ValidPhone + ", " + pdBusinessPhone.DNCValidPhone);
            }

            if (record.WidgetPhone != "")
            {
                PhoneDisposition pdWidgetPhone = new PhoneDisposition(record.WidgetPhone);
                Console.WriteLine(record.CaseSafeID + " (pdWidgetPhone):  " + pdWidgetPhone.Phone + ", " + pdWidgetPhone.Plus1Phone + ", " + pdWidgetPhone.HasExtension + ", " + pdWidgetPhone.ValidPhone + ", " + pdWidgetPhone.DNCValidPhone);
            }

            if (record.RegistrationPhone != "")
            {
                PhoneDisposition pdRegistrationPhone = new PhoneDisposition(record.RegistrationPhone);
                Console.WriteLine(record.CaseSafeID + " (RegistrationPhone):  " + pdRegistrationPhone.Phone + ", " + pdRegistrationPhone.Plus1Phone + ", " + pdRegistrationPhone.HasExtension + ", " + pdRegistrationPhone.ValidPhone + ", " + pdRegistrationPhone.DNCValidPhone);
            }

            return record;
        }

        private string DigitsOnly(string phone)
        {
            Regex digitsOnly = new Regex(@"[^\d]");

            return digitsOnly.Replace(phone, "");
        }

    }

}
