using System.Text.RegularExpressions;

namespace PhoneScrubber
{

    public class Disposition
    {
        // list of strings used to determine that a phone number contains an extension.
        public static readonly string[] extensionIndicators = new string[] { "ext", "ete", "x" };

        public string Phone { get; set; }
        public bool HasExtension { get; set; } = false;
        public bool DNCValidPhone { get; set; } = false;
        public bool Plus1Phone { get; set; } = false;
        public bool ValidPhone { get; set; } = false;
        public bool CannotBeParsed { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PhoneScrubber.Disposition"/> class, and determines the disposition of a phone number.
        /// </summary>
        /// <param name="phone">Phone number.</param>
        public Disposition(string phone)
        {
            this.Phone = phone;
            DetermineDisposition();
        }

        /// <summary>
        /// Determines the disposition of a phone number.
        /// </summary>
        /// <returns>The disposition of a phone number.</returns>
        public Disposition DetermineDisposition()
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

        /// <summary>
        /// Dispositions the phone number as containing an extension.
        /// </summary>
        /// <returns><c>true</c>, if a phone number is determined to contain an extension <c>false</c> otherwise.</returns>
        /// <param name="phone">Phone number.</param>
        public bool ContainsExtension(string phone)
        {
            bool result = false;

            if (phone.ContainsAny(extensionIndicators))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Dispositions the phone number as DNC valid.
        /// </summary>
        /// <returns><c>true</c>, if a phone number is DNC valie <c>false</c> otherwise.</returns>
        /// <param name="phone">Phone number.</param>
        public bool IsDNCValidPhone(string phone)
        {
            Regex vaidPhone = new Regex(@"^[0-9]{10}$");

            return vaidPhone.IsMatch(phone);
        }

        /// <summary>
        /// Dispositions the phone number as being in a generally valid format.
        /// </summary>
        /// <returns><c>true</c>, if a phone number is a gernerally valid format <c>false</c> otherwise.</returns>
        /// <param name="phone">Phone number.</param>
        public bool IsValidPhone(string phone)
        {
            Regex vaidPhone = new Regex(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");

            return vaidPhone.IsMatch(phone);
        }

        /// <summary>
        /// Dispositions the phone number as being in a +1.###-###-#### format.
        /// </summary>
        /// <returns><c>true</c>, if a phone number is a +1.###-###-#### format <c>false</c> otherwise.</returns>
        /// <param name="phone">Phone number.</param>
        public bool IsPlus1Phone(string phone)
        {
            Regex vaidPhone = new Regex(@"^([+][1][.])([0-9]{10})$");

            return vaidPhone.IsMatch(phone);
        }
    }

}

