using System;
using System.Globalization;
using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    [SetCulture("en-us")]
    public class TermResolverTest
    {
        [TermSettings(Format = ">>{0}")]
        public enum Options
        {
            [Term(TermCase.Title, Format = ">>{0}:")]
            A,
            B,
            C,
            [Term(TermCase.Lower, Format = "--{0}--")]
            D
        }

        [TestCaseSource(typeof(TermResolverFormatTestCaseSource))]
        public void TermResolver_Format(object value, string format, string expectedFormattedValue)
        {
            TermOptions options = new TermOptions { Format = format };
            string formatted = TermResolver.ToString(value, options);

            Assert.That(formatted, Is.EqualTo(expectedFormattedValue));

            object unformatted = TermResolver.FromString(formatted, value.GetType(), options);
            Assert.That(unformatted, Is.EqualTo(value));
        }

        private class TermResolverFormatTestCaseSource : TestCaseDataSource
        {
#pragma warning disable S1144 // Unused private types or members should be removed
            public TermResolverFormatTestCaseSource()
            {
                Add("test string");
                Add('a');
                Add(true);
                Add((byte)126);
                Add(50);
                Add(1234.56f);
                Add(1234.56, "C2", "$1,234.56");
                Add(1234.5, "asw {0:N1}_", "asw 1,234.5_");
                Add(0.25, "P0", "25 %");
                Add(-0.257f, "tax {0:P1}", "tax -25.7 %");
                Add(15, "Percent: {0}%");
                Add(Options.B);
                Add(Options.D, null, "--d--");
                Add(Options.A, null, ">>A:");
                Add(Options.C, "D", "2");
                Add(Options.C, "{0:D}.", "2.");
                Add(Options.C, "X", "00000002");
                Add(Options.A, "_{0:G}_", "_A_");

                DateTime date = new DateTime(2016, 3, 28);
                Add(date, "date: '{0:d}'");
                Add(date, "date: '{0:yyyy-MM-dd}'");
                Add(date, "MM/dd", "03/28");

                Add(new TimeSpan(10, 45, 15), "c", "10:45:15");
                Add(new TimeSpan(10, 45, 15), "time: '{0:g}'");
                Add(new TimeSpan(10, 45, 0), "time: '{0:hh\\:mm}'");
                Add(new TimeSpan(10, 45, 0), "hh:mm tt", "10:45 AM");
                Add(new TimeSpan(17, 45, 0), "time: {0:h\\:mm tt}", "time: 5:45 PM");

                Guid guid = new Guid("9d0aa4f2-4987-4395-be95-76abc329b7a0");
                Add(guid);
                Add(guid, "P", "(9d0aa4f2-4987-4395-be95-76abc329b7a0)");
                Add(guid, "<{0:B}>");
            }
#pragma warning restore S1144 // Unused private types or members should be removed

            private void Add(object value, string format = "<{0}>", string expectedFormattedValue = null)
            {
                base.Add(value, format, expectedFormattedValue ?? string.Format(new CultureInfo("en-us"), format, value));
            }
        }
    }
}
