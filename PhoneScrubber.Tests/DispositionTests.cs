using Xunit;
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
        public void HasExtension_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.HasExtension().ShouldBeTrue();
        }

        [Theory]
        [InlineData("(801)123-4567:1234")]
        [InlineData("FAX: 801 1234567")]
        [InlineData("801 123/4567.1234")]
        [InlineData("+1(916) 242-0144")]
        public void HasExtension_ShouldBeFalse(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.HasExtension().ShouldBeFalse();
        }

        [Theory]
        [InlineData("7274003253")]
        public void IsDNCValidPhone_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.DNCValidPhone().ShouldBeTrue();
        }

        [Theory]
        [InlineData("17274003253")]
        [InlineData("774003253")]
        [InlineData("(727) 400-3253")]
        [InlineData("1801 1234567")]
        [InlineData("(801)1234567")]
        [InlineData("982 009-8200")]
        [InlineData("+1(503)941-6610")]
        [InlineData("(503)941-6610")]
        [InlineData("(727) 400.3253")]
        [InlineData("(727) 400 3253")]
        [InlineData("(503) 9416610")]
        [InlineData("1(727) 400-3253")]
        [InlineData("(727)400 3253")]
        public void IsDNCValidPhone_ShouldBeFalse(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.DNCValidPhone().ShouldBeFalse();
        }

        [Theory]
        [InlineData("+1.0123456789")]
        public void IsPlus1Phone_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.Plus1Phone().ShouldBeTrue();
        }

        [Theory]
        [InlineData("1(801)123.4567")]
        [InlineData("1801 1234567")]
        [InlineData("1.8011234567")]
        [InlineData("+18011234567")]
        [InlineData("01234567890")]
        [InlineData(".0123456789")]
        [InlineData("+1.01234567890")]
        [InlineData("+1.012345678")]
        public void IsPlus1Phone_ShouldBeFalse(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.Plus1Phone().ShouldBeFalse();
        }

        [Theory]
        [InlineData("1(801)123.4567")]
        [InlineData("+1.801.123.4567")]
        [InlineData("801-123-4567")]
        [InlineData("1801 1234567")]
        [InlineData("801 1234567")]
        [InlineData("9820098200")]
        [InlineData("+1.5039416610")]
        public void IsValidPhone_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.ValidPhone().ShouldBeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("1(123)-123-123")]
        public void TooFewDigits_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.TooFewDigits().ShouldBeTrue();
        }

        [Theory]
        [InlineData("1(123)CUP-CAKE")]
        [InlineData("1234567890")]
        [InlineData("123abc4567890")]
        [InlineData("123-456-7890")]
        [InlineData("1(123)456-7890")]
        public void TooFewDigits_ShouldBeFalse(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.TooFewDigits().ShouldBeFalse();
        }

        [Theory]
        [InlineData("123 N. 6000 West, Hawaii 89231")]
        public void IsAddress_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.IsAddress().ShouldBeTrue();
        }

        [Theory]
        [InlineData("1234567890")]
        public void IsAddress_ShouldBeFalse(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.IsAddress().ShouldBeFalse();
        }

        [Theory]
        [InlineData("jgotti@marketstar.com")]
        [InlineData("joe.doe@mail.tv")]
        [InlineData("1234567890@mail.org")]
        [InlineData("joseph.e.smithson@mail.info")]
        private void IsEmailAddress_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.IsEmailAddress().ShouldBeTrue();
        }

        [Theory]
        [InlineData("1234@567890.")]
        private void IsEmailAddress_ShouldBeFalse(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.IsEmailAddress().ShouldBeFalse();
        }

        [Theory]
        [InlineData("1(123)CUP-CAKE")]
        [InlineData("123 SUP-DUDE")]
        [InlineData("1-123-KID-YOLO")]
        [InlineData("123-COMCAST")]
        private void IsAlphaPhone_ShouldBeTrue(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.IsAlphaPhone().ShouldBeTrue();
        }

        [Theory]
        [InlineData("FAX: 801 1234567")]
        [InlineData("OR CALL 1800123456")]
        [InlineData("1-123-YOLOExtension12")]
        private void IsAlphaPhone_ShouldBeFalse(string value)
        {
            Disposition disposition = new Disposition(value);
            disposition.IsAlphaPhone().ShouldBeFalse();
        }
    }
}