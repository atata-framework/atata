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
                case TermFormat.Title:
                    return value.Humanize(LetterCasing.Title);
                case TermFormat.Sentence:
                    return value.Humanize(LetterCasing.Sentence);
                case TermFormat.LowerCase:
                    return value.Humanize(LetterCasing.LowerCase);
                case TermFormat.UpperCase:
                    return value.Humanize(LetterCasing.AllCaps);
                case TermFormat.Camel:
                    return value.Humanize().Camelize();
                case TermFormat.Pascal:
                    return value.Humanize().Pascalize();
                case TermFormat.Dashed:
                    return value.Humanize().Dasherize();
                case TermFormat.XDashed:
                    return "x-" + value.Humanize().Dasherize();
                case TermFormat.Underscored:
                    return value.Humanize().Underscore();
                default:
                    throw ExceptionsFactory.CreateForUnsuppotedEnumValue(format, "format");
            }
        }
    }
}
