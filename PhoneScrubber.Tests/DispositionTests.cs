﻿using Xunit;
using Shouldly;

namespace PhoneScrubber.Tests
{
  public class DispositionTests
  {
    [Theory]
    [InlineData("(801)123-4567x1234")]
    [InlineData("801 1234567x1234")]
    [InlineData("801 123/4567ete1234")]
    [InlineData("801 123.4567ext1234")]
    [InlineData("(916) 242-0144 use ext. 2")]
    public void HasExtension_ShouldBeTrue(string phone)
    {
      Disposition disposition = new Disposition(phone);
      disposition.HasExtension().ShouldBeTrue();
    }

    [Theory]
    [InlineData("(801)123-4567")]
    [InlineData("801.123.4567")]
    [InlineData("801-123-4567")]
    public void HasExtension_ShouldBeFalse(string phone)
    {
      var disposition = new Disposition(phone);
      disposition.HasExtension().ShouldBeFalse();
    }

    [Theory]
    [InlineData("7274003253", true)]
    [InlineData("17274003253", false)]
    [InlineData("774003253", false)]
    [InlineData("(727) 400-3253", false)]
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
      Disposition disposition = new Disposition(phone);
      disposition.DNCValidPhone().ShouldBe(expected);
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
      Disposition disposition = new Disposition(phone);
      disposition.Plus1Phone().ShouldBe(expected);
    }

    [Theory]
    [InlineData("1(801)123.4567", true)]
    [InlineData("+1.801.123.4567", true)]
    [InlineData("801-123-4567", true)]
    [InlineData("1801 1234567", true)]
    [InlineData("801 1234567", true)]
    [InlineData("9820098200", true)]
    [InlineData("+1.5039416610", true)]
    public void TestIsValidPhone(string phone, bool expected)
    {
      Disposition disposition = new Disposition(phone);
      disposition.ValidPhone().ShouldBe(expected);
    }
  }
}
