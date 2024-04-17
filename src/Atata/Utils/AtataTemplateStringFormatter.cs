namespace Atata;

internal sealed class AtataTemplateStringFormatter : IFormatProvider, ICustomFormatter
{
    private const string InnerFormatValueIndicator = "*";

    private AtataTemplateStringFormatter()
    {
    }

    public static AtataTemplateStringFormatter Default { get; } = new AtataTemplateStringFormatter();

    public object GetFormat(Type formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (arg is null)
            return string.Empty;

        if (!string.IsNullOrEmpty(format))
        {
            if (arg is IFormattable argFormattable)
                return argFormattable.ToString(format, CultureInfo.CurrentCulture);

            string argumentAsString = arg as string ?? arg.ToString();
            return FormatInnerString(format, argumentAsString);
        }
        else
        {
            return arg.ToString();
        }
    }

    private static string FormatInnerString(string format, string argument)
    {
        string normalizedFormat = format
            .Replace(InnerFormatValueIndicator, "{0}")
            .Replace("{0}{0}", InnerFormatValueIndicator);

        return string.Format(normalizedFormat, argument);
    }
}
