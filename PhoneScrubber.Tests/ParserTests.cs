using Xunit;
using PhoneScrubber;

namespace PhoneScurbberTests
{
    public class ParsePhoneTests
    {
        [Theory]
        [InlineData("(801)123-4567", "8011234567")]
        [InlineData("801.123.4567", "8011234567")]
        [InlineData("801 123/4567x1234", "80112345671234")]
        [InlineData("+1.0123456789", "10123456789")]
        public void TestDigitsOnly(string phone, string expected)
        {
            Parser parser = new Parser();

            Assert.Matches(parser.DigitsOnly(phone), expected);
        }

        [Theory]
        [InlineData("(801)123-4567", false)]
        [InlineData("801.123.4567",  false)]
        [InlineData("801 1234567x1234", true)]
        [InlineData("801 123/4567extension1234", true)]
        [InlineData("801 123.4567ext1234", true)]
        public void TestHasExtension(string phone, bool expected)
        {
            Parser parser = new Parser();

            Assert.Equal(parser.HasExtension(phone), expected);
        }

        [Theory]
        [InlineData("(727) 400-3253", true)]
        [InlineData("1801 1234567", false)]
        [InlineData("(801)1234567", false)]
        [InlineData("982 009-8200", false)]
        [InlineData("+1(503)941-6610", false)]
        [InlineData("(503)941-6610", false)]
        [InlineData("(727) 400.3253", false)]
        [InlineData("(727) 400 3253", false)]
        [InlineData("(503) 9416610", false)]
        [InlineData("1(727) 400-3253", false)]
        [InlineData("(727)400 3253", false)]
        public void TestIsDNCValidPhone(string phone, bool expected)
        {
            Parser parser = new Parser();

            Assert.Equal(parser.IsDNCValidPhone(phone), expected);
        }

        [Theory]
        [InlineData("+1.0123456789", true)]
        [InlineData("1(801)123.4567", false)]
        [InlineData("1801 1234567", false)]
        [InlineData("1.8011234567", false)]
        [InlineData("+18011234567", false)]
        [InlineData("01234567890", false)]
        [InlineData(".0123456789", false)]
        [InlineData("+1.01234567890", false)]
        [InlineData("+1.012345678", false)]
        public void TestIsPlus1Phone(string phone, bool expected)
        {
            Parser parser = new Parser();

            Assert.Equal(parser.IsPlus1Phone(phone), expected);
        }

        [Theory]
        [InlineData("1(801)123.4567", true)]
        [InlineData("1801 1234567", true)]
        [InlineData("801 1234567", true)]
        [InlineData("9820098200", true)]
        [InlineData("+1.5039416610", true)]
        public void TestIsValidPhone(string phone, bool expected)
        {
            Parser parser = new Parser();

            Assert.Equal(parser.IsValidPhone(phone), expected);
        }
    }
}
