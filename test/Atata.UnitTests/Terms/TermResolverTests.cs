using System.Globalization;

namespace Atata.UnitTests.Terms;

public static class TermResolverTests
{
    private const string CultureName = "en-GB";

    [TestFixture]
    [SetCulture(CultureName)]
    public class FromString
    {
        public enum Option
        {
            [Term(TermCase.Title, Format = ">>{0}:")]
            A,
            B,
            C,
            [Term(TermCase.Lower, Format = "--{0}--")]
            D
        }

        [TermSettings(Format = "++{0}")]
        public enum OptionWithTermSettingsFormat
        {
            [Term(TermCase.Title, Format = "+{0}:")]
            SA,
            SB,
            SC,
            [Term(TermCase.Lower, Format = "--{0}--")]
            SD
        }

        public enum TermCasedOption
        {
            RegularValue,

            [Term(TermCase.Camel)]
            CamelUsingCase,

            [Term("camelUsingTerm")]
            CamelUsingTerm,

            [Term(TermCase.Pascal)]
            PascalUsingCase,

            [Term("PascalUsingTerm")]
            PascalUsingTerm,

            [Term(TermCase.Title)]
            TitleUsingCase,

            [Term("Title Using Term")]
            TitleUsingTerm,

            [Term(TermCase.Sentence)]
            SentenceUsingCase,

            [Term("Sentence using term")]
            SentenceUsingTerm,

            [Term(TermCase.MidSentence)]
            MidSentenceUsingCase,

            [Term("mid-sentence using term")]
            MidSentenceUsingTerm
        }

        [TestCase("true", ExpectedResult = true)]
        [TestCase("True", ExpectedResult = true)]
        [TestCase("false", ExpectedResult = false)]
        [TestCase("False", ExpectedResult = false)]
        [TestCase("", ExpectedResult = null)]
        [TestCase(null, ExpectedResult = null)]
        public bool? NullableBool(string value) =>
            TermResolver.FromString<bool?>(value);

        [TestCaseSource(typeof(TermResolverFormatTestCaseSource))]
        public void WithFormat(object value, string format, string expectedFormattedValue)
        {
            TermOptions options = new() { Format = format };
            string formatted = TermResolver.ToString(value, options);

            Assert.That(formatted, Is.EqualTo(expectedFormattedValue));

            object unformatted = TermResolver.FromString(formatted, value.GetType(), options);
            Assert.That(unformatted, Is.EqualTo(value));
        }

        [TestCase(TermCasedOption.RegularValue, "Regular Value")]
        [TestCase(TermCasedOption.CamelUsingCase, "camelUsingCase")]
        [TestCase(TermCasedOption.CamelUsingTerm, "camelUsingTerm")]
        [TestCase(TermCasedOption.PascalUsingCase, "PascalUsingCase")]
        [TestCase(TermCasedOption.PascalUsingTerm, "PascalUsingTerm")]
        [TestCase(TermCasedOption.TitleUsingCase, "Title Using Case")]
        [TestCase(TermCasedOption.TitleUsingTerm, "Title Using Term")]
        [TestCase(TermCasedOption.SentenceUsingCase, "Sentence using case")]
        [TestCase(TermCasedOption.SentenceUsingTerm, "Sentence using term")]
        [TestCase(TermCasedOption.MidSentenceUsingCase, "mid sentence using case")]
        [TestCase(TermCasedOption.MidSentenceUsingTerm, "mid-sentence using term")]
        public void Enum(TermCasedOption value, string expectedValue)
        {
            string resolvedString = TermResolver.ToString(value);

            Assert.That(resolvedString, Is.EqualTo(expectedValue));

            TermCasedOption resolvedBack = TermResolver.FromString<TermCasedOption>(resolvedString);
            Assert.That(resolvedBack, Is.EqualTo(value));
        }

        [TestCase(TermCasedOption.RegularValue, TermCase.MidSentence, "regular value")]
        [TestCase(TermCasedOption.CamelUsingCase, TermCase.MidSentence, "camelUsingCase")]
        [TestCase(TermCasedOption.CamelUsingTerm, TermCase.MidSentence, "camelUsingTerm")]
        [TestCase(TermCasedOption.PascalUsingCase, TermCase.MidSentence, "pascalUsingCase")]
        [TestCase(TermCasedOption.PascalUsingTerm, TermCase.MidSentence, "pascalUsingTerm")]
        [TestCase(TermCasedOption.TitleUsingCase, TermCase.MidSentence, "title using case")]
        [TestCase(TermCasedOption.TitleUsingTerm, TermCase.MidSentence, "title using term")]
        [TestCase(TermCasedOption.SentenceUsingCase, TermCase.MidSentence, "sentence using case")]
        [TestCase(TermCasedOption.SentenceUsingTerm, TermCase.MidSentence, "sentence using term")]
        [TestCase(TermCasedOption.MidSentenceUsingCase, TermCase.MidSentence, "mid sentence using case")]
        [TestCase(TermCasedOption.MidSentenceUsingTerm, TermCase.MidSentence, "mid-sentence using term")]
        public void Enum_WithCase(TermCasedOption value, TermCase termCase, string expectedValue)
        {
            TermOptions options = new() { Case = termCase };
            string resolvedString = TermResolver.ToString(value, options);

            Assert.That(resolvedString, Is.EqualTo(expectedValue));

            TermCasedOption resolvedBack = TermResolver.FromString<TermCasedOption>(resolvedString, options);
            Assert.That(resolvedBack, Is.EqualTo(value));
        }

#pragma warning disable S1144, CA1812 // Unused private types or members should be removed
        private class TermResolverFormatTestCaseSource : TestCaseDataSource
        {
            public TermResolverFormatTestCaseSource()
            {
                Add("test string");
                Add('a');
                Add(true);
                Add((byte)126);
                Add(50);
                Add(1234.56f);
                Add(1234.56, "C2", "£1,234.56");
                Add(1234.5, "asw {0:N1}_", "asw 1,234.5_");
                Add(0.25, "P0", "25%");
                Add(-0.257f, "tax {0:P1}", "tax -25.7%");
                Add(15, "Percent: {0}%");
                Add("txt", "{{{0}}}");
                Add("txt", "{{{{{0}}}}}");
                Add("txt", "}}}}}}{0}{{{{{{");

                Add(Option.B);
                Add(Option.D, null, "--d--");
                Add(Option.A, null, ">>A:");
                Add(Option.C, "D", "2");
                Add(Option.C, "{0:D}.", "2.");
                Add(Option.C, "X", "00000002");
                Add(Option.A, "_{0:G}_", "_>>A:_");

                Add(OptionWithTermSettingsFormat.SB, "<{0}>", "<++SB>");
                Add(OptionWithTermSettingsFormat.SD, null, "--sd--");
                Add(OptionWithTermSettingsFormat.SA, null, "+SA:");
                Add(OptionWithTermSettingsFormat.SC, "D", "++SC");
                Add(OptionWithTermSettingsFormat.SC, "{0:D}.", "++SC.");
                Add(OptionWithTermSettingsFormat.SA, "_{0:G}_", "_+SA:_");

                DateTime date = new(DateTime.Today.Year, 3, 28);
                Add(date, "date: '{0:d}'");
                Add(date, "date: '{0:yyyy-MM-dd}'");
                Add(date, "MM/dd", "03/28");

                Add(new TimeSpan(10, 45, 15), "c", "10:45:15");
                Add(new TimeSpan(10, 45, 15), "time: '{0:g}'");
                Add(new TimeSpan(10, 45, 0), "time: '{0:hh\\:mm}'");
                Add(new TimeSpan(10, 45, 0), "hh:mm tt", "10:45 am");
                Add(new TimeSpan(17, 45, 0), "time: {0:h\\:mm tt}", "time: 5:45 pm");

                Guid guid = new("9d0aa4f2-4987-4395-be95-76abc329b7a0");
                Add(guid);
                Add(guid, "P", "(9d0aa4f2-4987-4395-be95-76abc329b7a0)");
                Add(guid, "<{0:B}>");
            }

            private void Add(object value, string format = "<{0}>", string expectedFormattedValue = null) =>
                base.Add(value, format, expectedFormattedValue ?? string.Format(CultureInfo.GetCultureInfo(CultureName), format, value));
        }
#pragma warning restore S1144, CA1812 // Unused private types or members should be removed
    }

    [TestFixture]
    [SetCulture(CultureName)]
    public new class ToString
    {
        [TestCase(true, ExpectedResult = "True")]
        [TestCase(false, ExpectedResult = "False")]
        public string Bool(object value) =>
            TermResolver.ToString(value);
    }
}
