namespace Atata;

internal sealed class AtataTemplateStringFormatter : IFormatProvider, ICustomFormatter
{
    private const string InnerFormatValueIndicator = "*";

    private readonly Func<string, string>? _argumentHandler;

    internal AtataTemplateStringFormatter(Func<string, string>? argumentHandler = null) =>
        _argumentHandler = argumentHandler;

    public static AtataTemplateStringFormatter Default { get; } = new();

    public object? GetFormat(Type formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (arg is null)
            return string.Empty;

        if (format?.Length > 0)
        {
            if (arg is IFormattable argFormattable)
            {
                string argumentAsString = argFormattable.ToString(format, CultureInfo.CurrentCulture);

                return _argumentHandler is null
                    ? argumentAsString
                    : _argumentHandler.Invoke(argumentAsString);
            }
            else
            {
                string argumentAsString = arg as string ?? arg.ToString();

                if (_argumentHandler is not null)
                    argumentAsString = _argumentHandler.Invoke(argumentAsString);

                return FormatInnerString(format, argumentAsString);
            }
        }
        else
        {
            string argumentAsString = arg as string ?? arg.ToString();

            return _argumentHandler is null
                ? argumentAsString
                : _argumentHandler.Invoke(argumentAsString);
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
