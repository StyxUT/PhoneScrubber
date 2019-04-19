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
    private void TestDigitsOnly(string phone, string expected)
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
      d.Phone.ShouldBe(expected);
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
      d.Phone.ShouldBe(expected);
    }

    [Theory]
    [InlineData("1 (801)123-4567", "8011234567")]
    [InlineData("(801)123-4567", "8011234567")]
    [InlineData("(916) 242-0144 use ext. 2", "9162420144")]
    [InlineData("1-800-123-4567", "18001234567")]
    [InlineData("1-800-GOT-CHKS", "18004682457")]
    private void ParsePhone_ShouldPass(string phone, string expected)
    {
      // arrange
      var d = new Disposition(phone);

      // act
      var result = Parser.ParsePhone(d);

      // assert
      result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("51 205 3594")]
    private void ParsePhone_TooFewDigits_ShouldFail(string phone)
    {
      // arrange
      var d = new Disposition(phone);

      // act
      var result = Parser.ParsePhone(d);

      // assert
      result.ShouldBe("Too few digits.");
    }

    [Theory]
    [InlineData("1-800-999-99999")]
    private void ParsePhone_CouldNotParse_ShouldFail(string phone)
    {
      // arrange
      var d = new Disposition(phone);

      // act
      var result = Parser.ParsePhone(d);

      // assert
      result.ShouldBe("Could not parse.");
    }
  }
}
