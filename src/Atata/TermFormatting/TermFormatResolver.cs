using System.Collections.Generic;
using System.Linq;

namespace Atata.TermFormatting
{
    public static class TermFormatResolver
    {
        private static readonly Dictionary<TermFormat, FormatterItem> Formatters;

        static TermFormatResolver()
        {
            Formatters = new Dictionary<TermFormat, FormatterItem>
            {
                { TermFormat.Title, FormatterItem.For<TitleTermFormatter>() },
                { TermFormat.TitleWithColon, FormatterItem.For<TitleTermFormatter>("{0}:") },
                { TermFormat.Sentence, FormatterItem.For<SentenceTermFormatter>() },
                { TermFormat.SentenceLower, FormatterItem.For<SentenceLowerTermFormatter>() },
                { TermFormat.SentenceWithColon, FormatterItem.For<SentenceTermFormatter>("{0}:") },
                { TermFormat.Lower, FormatterItem.For<LowerTermFormatter>() },
                { TermFormat.LowerMerged, FormatterItem.For<LowerMergedTermFormatter>() },
                { TermFormat.Upper, FormatterItem.For<UpperTermFormatter>() },
                { TermFormat.UpperMerged, FormatterItem.For<UpperMergedTermFormatter>() },
                { TermFormat.Camel, FormatterItem.For<CamelTermFormatter>() },
                { TermFormat.Pascal, FormatterItem.For<PascalTermFormatter>() },
                { TermFormat.Kebab, FormatterItem.For<KebabTermFormatter>() },
                { TermFormat.XKebab, FormatterItem.For<KebabTermFormatter>("x-{0}") },
                { TermFormat.HyphenKebab, FormatterItem.For<HyphenKebabTermFormatter>() },
                { TermFormat.Snake, FormatterItem.For<SnakeTermFormatter>() },
                { TermFormat.PascalKebab, FormatterItem.For<PascalKebabTermFormatter>() },
                { TermFormat.PascalHyphenKebab, FormatterItem.For<PascalHyphenKebabTermFormatter>() }
            };
        }

        public static string ApplyFormat(string value, TermFormat format)
        {
            if (format == TermFormat.None || format == TermFormat.Inherit)
                return value;

            string[] words = value.SplitIntoWords();

            if (!words.Any())
                return string.Empty;

            FormatterItem formatterItem;
            if (!Formatters.TryGetValue(format, out formatterItem))
                throw ExceptionFactory.CreateForUnsupportedEnumValue(format, "format");

            string formattedValue = formatterItem.Formatter.Format(words);

            if (!string.IsNullOrWhiteSpace(formatterItem.StringFormat))
                formattedValue = string.Format(formatterItem.StringFormat, formattedValue);

            return formattedValue;
        }

        private class FormatterItem
        {
            public FormatterItem(ITermFormatter formatter, string stringFormat = null)
            {
                Formatter = formatter;
                StringFormat = stringFormat;
            }

            public ITermFormatter Formatter { get; private set; }
            public string StringFormat { get; private set; }

            public static FormatterItem For<T>(string stringFormat = null)
                where T : ITermFormatter, new()
            {
                ITermFormatter formatter = new T();
                return new FormatterItem(formatter, stringFormat);
            }
        }
    }
}
