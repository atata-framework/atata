#nullable enable

namespace Atata;

internal sealed class AtataUriTemplateStringFormatter : IFormatProvider, ICustomFormatter
{
    private static readonly Func<string, string> s_defaultModifierFunction =
        Uri.EscapeDataString;

    private static readonly Dictionary<string, Func<string, string>> s_aliasAndModifierFunctionMap =
        new()
        {
            ["noescape"] = x => x,
            ["uriescape"] = Uri.EscapeUriString,
            ["dataescape"] = Uri.EscapeDataString,
        };

    private readonly AtataTemplateStringFormatter _templateFormatter = AtataTemplateStringFormatter.Default;

    private AtataUriTemplateStringFormatter()
    {
    }

    public static AtataUriTemplateStringFormatter Default { get; } = new();

    public object? GetFormat(Type formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (arg is null)
            return string.Empty;

        var processingResult = ProcessFormat(format);

        object argument = processingResult.EncodeFirst
            ? processingResult.EncodeFunction.Invoke(arg.ToString())
            : arg;

        string origin = _templateFormatter.Format(processingResult.LeftFormat, argument, formatProvider);

        return processingResult.EncodeFirst
            ? origin
            : processingResult.EncodeFunction.Invoke(origin);
    }

    private static UriVariableProcessingResult ProcessFormat(string format)
    {
        if (!string.IsNullOrEmpty(format))
        {
            foreach (var item in s_aliasAndModifierFunctionMap)
            {
                if (format.Equals(item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    return new UriVariableProcessingResult(string.Empty, item.Value, true);
                }
                else if (format.StartsWith(item.Key + ':', StringComparison.OrdinalIgnoreCase))
                {
                    string leftFormat = format[(item.Key.Length + 1)..];
                    return new UriVariableProcessingResult(leftFormat, item.Value, true);
                }
                else if (format.EndsWith(':' + item.Key, StringComparison.OrdinalIgnoreCase))
                {
                    string leftFormat = format[..(format.Length - item.Key.Length - 1)];
                    return new UriVariableProcessingResult(leftFormat, item.Value, false);
                }
            }
        }

        return new(format, s_defaultModifierFunction, false);
    }

    private sealed class UriVariableProcessingResult
    {
        public UriVariableProcessingResult(
            string leftFormat,
            Func<string, string> encodeFunction,
            bool encodeFirst)
        {
            LeftFormat = leftFormat;
            EncodeFunction = encodeFunction;
            EncodeFirst = encodeFirst;
        }

        public string LeftFormat { get; }

        public Func<string, string> EncodeFunction { get; }

        public bool EncodeFirst { get; }
    }
}
