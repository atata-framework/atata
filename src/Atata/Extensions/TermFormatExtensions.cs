using Atata.TermFormatting;
using Humanizer;

namespace Atata
{
    public static class TermFormatExtensions
    {
        public static string ApplyTo(this TermFormat format, string value)
        {
            switch (format)
            {
                case TermFormat.None:
                    return value;
                case TermFormat.TitleWithColon:
                    return value.Humanize(LetterCasing.Title) + ":";
                case TermFormat.Sentence:
                    return value.Humanize(LetterCasing.Sentence);
                case TermFormat.SentenceWithColon:
                    return value.Humanize(LetterCasing.Sentence) + ":";
                case TermFormat.LowerCase:
                    return value.Humanize(LetterCasing.LowerCase);
                case TermFormat.UpperCase:
                    return value.Humanize(LetterCasing.AllCaps);
                case TermFormat.Pascal:
                    return value.Pascalize();
                case TermFormat.Hyphenated:
                    return value.Underscore().Hyphenate();
                case TermFormat.PascalDashed:
                    return value.Underscore().PascalDasherize();
                case TermFormat.PascalHyphenated:
                    return value.Underscore().PascalHyphenate();
                case TermFormat.Title:
                case TermFormat.Camel:
                case TermFormat.Dashed:
                case TermFormat.XDashed:
                case TermFormat.Underscored:
                    return TermFormatResolver.ApplyFormat(value, format);
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(format, "format");
            }
        }
    }
}
