using System.Text.RegularExpressions;

namespace PhoneScrubber
{
    public class PhoneDisposition
    {
        public string Phone { get; set; }
        public bool HasExtension { get; set; } = false;
        public bool DNCValidPhone { get; set; } = false;
        public bool Plus1Phone { get; set; } = false;
        public bool ValidPhone { get; set; } = false;

        public PhoneDisposition(string phone)
        {
            this.Phone = phone;
            DetermineDisposition();
        }

        public PhoneDisposition DetermineDisposition()
        {

            if (this.Phone != null)
            {
                this.HasExtension = ContainsExtension(this.Phone);
                this.Plus1Phone = IsPlus1Phone(this.Phone);
                this.ValidPhone = IsValidPhone(this.Phone);
                this.DNCValidPhone = IsDNCValidPhone(this.Phone);
            }

            return this;
        }
        private bool ContainsExtension(string phone)
        {
            bool result = false;

            string ext = "ext";
            string extension = "extension";
            string x = "x";

            if (phone.ContainsAny(ext, extension, x))
            {
                result = true;
            }

            return result;
        }

        private bool IsDNCValidPhone(string phone)
        {
            Regex vaidPhone = new Regex(@"^\(([0-9]{3})\)[ ]([0-9]{3})[-]([0-9]{4})$");

            return vaidPhone.IsMatch(phone);
        }

        private bool IsValidPhone(string phone)
        {
            Regex vaidPhone = new Regex(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");

            return vaidPhone.IsMatch(phone); ;
        }

        private bool IsPlus1Phone(string phone)
        {
            Regex vaidPhone = new Regex(@"^([+][1][.])([0-9]{10})$");

            return vaidPhone.IsMatch(phone);
        }
    }

}

