using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    [SetCulture("en-us")]
    public class TermResolverTest
    {
        [TermSettings(StringFormat = ">>{0}")]
        public enum Options
        {
            [Term(TermFormat.TitleWithColon)]
            A,
            B,
            C,
            [Term(TermFormat.LowerCase, StringFormat = "--{0}--")]
            D
        }

        [TestCase("test string")]
        [TestCase(50)]
        [TestCase(1234.56f)]
        [TestCase(1234.56, "C2", "$1,234.56")]
        [TestCase(1234.5, "asw {0:N1}_", "asw 1,234.5_")]
        [TestCase(0.25, "P0", "25 %")]
        [TestCase(-0.257f, "tax {0:P1}", "tax -25.7 %")]
        [TestCase(15, "Percent: {0}%")]
        [TestCase(Options.B)]
        [TestCase(Options.D, null, "--d--")]
        [TestCase(Options.A, null, ">>A:")]
        [TestCase(Options.C, "D", "2")]
        [TestCase(Options.C, "{0:D}.", "2.")]
        [TestCase(Options.C, "X", "00000002")]
        [TestCase(Options.A, "_{0:G}_", "_A_")]
        public void TermResolver_StringFormat(object value, string format = "Before {0} after", string expectedFormattedValue = null)
        {
            TermOptions options = new TermOptions { StringFormat = format };
            string formatted = TermResolver.ToString(value, options);

            if (expectedFormattedValue == null)
                expectedFormattedValue = string.Format(format, value);
            Assert.That(formatted, Is.EqualTo(expectedFormattedValue));

            object unformatted = TermResolver.FromString(formatted, value.GetType(), options);
            Assert.That(unformatted, Is.EqualTo(value));
        }
    }
}
