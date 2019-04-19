using System.Text.RegularExpressions;

namespace PhoneScrubber
{

    public class Disposition
    {
        // list of strings used to determine that a phone number contains an extension.
        public static string[] ExtensionIndicators = { "ext", "ete", "x" };

        public string Value { get; set; }
        public bool CannotBeParsed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:PhoneScrubber.Disposition"/> class, and determines the disposition of a phone number.
        /// </summary>
        /// <param name="value">Phone number.</param>
        public Disposition(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Check to see if the phone has a Plus1.
        /// </summary>
        public bool Plus1Phone() => this.RegExIsMatch(@"^([+][1][.])([0-9]{10})$");
        /// <summary>
        /// Check to see if the phone has an extension.
        /// </summary>
        public bool HasExtension() => this.Value.ContainsAny(ExtensionIndicators);
        /// <summary>
        /// Check to see if the phone number is DNC valid.
        /// </summary>
        public bool DNCValidPhone() => this.RegExIsMatch("^[0-9]{10}$");
        /// <summary>
        /// Check to see if the phone number is valid.
        /// </summary>
        public bool ValidPhone() => this.RegExIsMatch(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
        public bool IsEmailAddress() => this.RegExIsMatch(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
        public bool TooFewDigits() => this.ContainsTooFewDigits();
        public bool IsAddress() => this.RegExIsMatch(@"^[0-9]+\s+([a-zA-Z]+|[a-zA-Z]+\s[a-zA-Z]+)$");
        public bool IsAlphaPhone() => this.RegExIsMatch(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*([a-zA-Z]{3})[-. ]*([a-zA-Z]{4})(?: *x(\d+))?\s*$");

        /// <summary>
        /// Check if pattern matches this.Value.
        /// </summary>
        /// <returns><c>true</c>, if ex is match was reged, <c>false</c> otherwise.</returns>
        /// <param name="pattern">Pattern.</param>
        private bool RegExIsMatch(string pattern)
        {
            if (string.IsNullOrWhiteSpace(this.Value))
                return false;

            return Regex.IsMatch(this.Value, pattern);
        }

        private bool ContainsTooFewDigits()
        {
            if (Parser.DigitsOnly(this.Value).Length < 10)
            {
                this.CannotBeParsed = true;
                return true;
            }
            return false;
        }

        public void SetValue(string value)
        {
            this.CannotBeParsed = true;
            this.Value = value;
        }
    }
}

