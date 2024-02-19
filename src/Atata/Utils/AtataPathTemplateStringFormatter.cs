namespace Atata;

internal sealed class AtataPathTemplateStringFormatter : IFormatProvider, ICustomFormatter
{
    internal const char CharToReplaceWith = '_';

    private readonly AtataTemplateStringFormatter _templateFormatter = AtataTemplateStringFormatter.Default;

    private AtataPathTemplateStringFormatter()
    {
    }

    public static AtataPathTemplateStringFormatter Default { get; } = new AtataPathTemplateStringFormatter();

    public object GetFormat(Type formatType) =>
        formatType == typeof(ICustomFormatter) ? this : null;

    public string Format(string format, object arg, IFormatProvider formatProvider)
    {
        if (arg is null)
            return string.Empty;

        string origin = _templateFormatter.Format(format, arg, formatProvider);

        char[] result = new char[origin.Length];

        for (int i = 0; i < origin.Length; i++)
        {
            result[i] = origin[i] switch
            {
                '"' or
                '<' or
                '>' or
                '|' or
                '\u0000' or
                '\u0001' or
                '\u0002' or
                '\u0003' or
                '\u0004' or
                '\u0005' or
                '\u0006' or
                '\u0007' or
                '\u0008' or
                '\u0009' or
                '\u000a' or
                '\u000b' or
                '\u000c' or
                '\u000d' or
                '\u000e' or
                '\u000f' or
                '\u0010' or
                '\u0011' or
                '\u0012' or
                '\u0013' or
                '\u0014' or
                '\u0015' or
                '\u0016' or
                '\u0017' or
                '\u0018' or
                '\u0019' or
                '\u001a' or
                '\u001b' or
                '\u001c' or
                '\u001d' or
                '\u001e' or
                '\u001f' or
                ':' or
                '*' or
                '?' or
                '\\' or
                '/' => CharToReplaceWith,
                _ => origin[i],
            };
        }

        return new string(result);
    }
}
