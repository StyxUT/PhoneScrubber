using PhoneScrubber;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Shouldly;

namespace PhoneScurbberTests
{
    public class ParsePhoneTests
    {
        [Theory]
        [InlineData("(801)123-4567", "8011234567")]
        [InlineData("801.123.4567", "8011234567")]
        [InlineData("801 123/4567x1234", "80112345671234")]
        [InlineData("+1.0123456789", "10123456789")]
        private void DigitsOnly_ShouldPass(string phone, string expected)
        {
            // arrange
            // act
            var digits = Parser.DigitsOnly(phone);

            // assert
            digits.ShouldBe(expected);
        }

        [Theory]
        [InlineData("+1.8011234567", "8011234567")]
        private void ScrubPlus1Phone_ShouldPass(string phone, string expected)
        {
            // arrange
            var d = new Disposition(phone);

            // act
            Parser.ScrubPlus1Phone(d);

            // assert
            d.Value.ShouldBe(expected);
        }

        [Theory]
        [InlineData("(801)123-4567", "(801)123-4567")]
        [InlineData("8011234567x1234", "8011234567")]
        [InlineData("801 1234567extension1234", "801 1234567")]
        [InlineData("801 1234567ext1234", "801 1234567")]
        [InlineData("801.123.4567 x1234", "801.123.4567 ")]
        [InlineData("8011234567 x1234", "8011234567 ")]
        [InlineData("1-8011234567ext1234", "1-8011234567")]
        [InlineData("(916) 242-0144 use ext. 2", "(916) 242-0144 use ")]
        private void ScrubExtension_ShouldPass(string phone, string expected)
        {
            // arrange
            var d = new Disposition(phone);

            // act
            Parser.ScrubExtension(d);

            // assert
            d.Value.ShouldBe(expected);
        }

        [Theory]
        [InlineData("1 (801)123-4567", "8011234567")]
        [InlineData("(801)123-4567", "8011234567")]
        [InlineData("(916) 242-0144 use ext. 2", "9162420144")]
        [InlineData("1-800-123-4567", "8001234567")]
        [InlineData("800 eat-food", "8003283663")]
        [InlineData("1-900-GOT-CHKS", "9004682457")]
        private void ParsePhone_ShouldPass(string phone, string expected)
        {
            // arrange
            var d = new Disposition(phone);

            // act
            var result = Parser.ParsePhone(d);

            // assert
            result.ShouldBe(expected);
            d.CannotBeParsed.ShouldBeFalse();
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        private void ParsePhone_NullOrEmpty_ShouldFail(string phone)
        {
            // arrange
            var d = new Disposition(phone);

            // act
            var result = Parser.ParsePhone(d);

            // assert
            result.ShouldBe("Null or Empty");
            d.CannotBeParsed.ShouldBeTrue();
        }

        [Theory]
        [InlineData("2164 W 1100 S Syracuse UT, 84075")]
        private void ParsePhone_Value_ShouldFail(string value)
        {
            // arrange
            var d = new Disposition(value);

            // act
            var result = Parser.ParsePhone(d);

            // assert
            result.ShouldBe("Could not parse");
        }

    }
}
