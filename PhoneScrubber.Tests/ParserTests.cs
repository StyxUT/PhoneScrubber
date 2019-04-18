using PhoneScrubber;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace PhoneScurbberTests
{

  public class TestPlus1PhoneData : IEnumerable<object[]>
  {
    public IEnumerator<object[]> GetEnumerator()
    {
      // initiliazation of Disposition object, set Plus1Phone property, expected test result
      yield return new object[] { new Disposition("+1.8011234567"), "8011234567" };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }

  public class TestScrubExtensionData : IEnumerable<object[]>
  {
    public IEnumerator<object[]> GetEnumerator()
    {
      // initiliazation of Disposition object, set HasExtension property, expected test result
      yield return new object[] { new Disposition("8011234567x1234"), "8011234567" };
      yield return new object[] { new Disposition("801 1234567extension1234"), "801 1234567" };
      yield return new object[] { new Disposition("801 1234567ext1234"), "801 1234567" };
      yield return new object[] { new Disposition("801.123.4567 x1234"), "801.123.4567 " };
      yield return new object[] { new Disposition("8011234567 x1234"), "8011234567 " };
      yield return new object[] { new Disposition("1-8011234567ext1234"), "1-8011234567" };

    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
  }

  public class ParsePhoneTests
  {
    [Theory]
    [InlineData("(801)123-4567", "8011234567")]
    [InlineData("801.123.4567", "8011234567")]
    [InlineData("801 123/4567x1234", "80112345671234")]
    [InlineData("+1.0123456789", "10123456789")]
    private void TestDigitsOnly(string phone, string expected)
    {
      Parser parser = new Parser();

      Assert.Matches(expected, parser.DigitsOnly(phone));
    }

    [Theory]
    [ClassData(typeof(TestPlus1PhoneData))]
    private void TestScrubPlus1Phone(Disposition d, string expected)
    {
      Parser parser = new Parser();
      parser.ScrubPlus1Phone(d);
      Assert.Matches(expected, d.Phone);
    }

    [Theory]
    [ClassData(typeof(TestScrubExtensionData))]
    private void TestScrubExtension(Disposition d, string expected)
    {
      Parser parser = new Parser();
      parser.ScrubExtension(d);

      Assert.Matches(expected, d.Phone);
    }
  }
}
