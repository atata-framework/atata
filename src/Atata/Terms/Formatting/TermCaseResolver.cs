using System.Collections.Generic;
using System.Linq;

namespace Atata.TermFormatting
{
    public static class TermCaseResolver
    {
        private static readonly Dictionary<TermCase, FormatterItem> Formatters = new Dictionary<TermCase, FormatterItem>
        {
            [TermCase.Title] = FormatterItem.For<TitleTermFormatter>(),
            [TermCase.Capitalized] = FormatterItem.For<CapitalizedTermFormatter>(),
            [TermCase.Sentence] = FormatterItem.For<SentenceTermFormatter>(),
            [TermCase.MidSentence] = FormatterItem.For<MidSentenceTermFormatter>(),
            [TermCase.Lower] = FormatterItem.For<LowerTermFormatter>(),
            [TermCase.LowerMerged] = FormatterItem.For<LowerMergedTermFormatter>(),
            [TermCase.Upper] = FormatterItem.For<UpperTermFormatter>(),
            [TermCase.UpperMerged] = FormatterItem.For<UpperMergedTermFormatter>(),
            [TermCase.Camel] = FormatterItem.For<CamelTermFormatter>(),
            [TermCase.Pascal] = FormatterItem.For<PascalTermFormatter>(),
            [TermCase.Kebab] = FormatterItem.For<KebabTermFormatter>(),
            [TermCase.HyphenKebab] = FormatterItem.For<HyphenKebabTermFormatter>(),
            [TermCase.Snake] = FormatterItem.For<SnakeTermFormatter>(),
            [TermCase.PascalKebab] = FormatterItem.For<PascalKebabTermFormatter>(),
            [TermCase.PascalHyphenKebab] = FormatterItem.For<PascalHyphenKebabTermFormatter>()
        };

        public static string ApplyCase(string value, TermCase termCase)
        {
            if (termCase == TermCase.None)
                return value;

            string[] words = value.SplitIntoWords();

            if (!words.Any())
                return string.Empty;

            FormatterItem formatterItem;
            if (!Formatters.TryGetValue(termCase, out formatterItem))
                throw ExceptionFactory.CreateForUnsupportedEnumValue(termCase, nameof(termCase));

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
