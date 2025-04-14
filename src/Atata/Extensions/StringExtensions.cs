namespace Atata;

public static class StringExtensions
{
    private static char[]? s_invalidFileNameChars;

    private static char[]? s_invalidPathChars;

    internal static string FormatWith(this string format, params object[] args) =>
        string.Format(format, args);

    public static string Prepend(this string value, string valueToPrepend) =>
        string.Concat(valueToPrepend, value);

    public static string Append(this string value, string valueToAppend) =>
        string.Concat(value, valueToAppend);

    public static bool IsUpper(this string value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));
        else if (value is [])
            return false;
        else
            return value.ToCharArray().All(char.IsUpper);
    }

    public static string ToUpperFirstLetter(this string value, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        if (value is null)
            throw new ArgumentNullException(nameof(value));
        else if (value.Length > 1)
            return char.ToUpper(value[0], culture) + value[1..];
        else
            return value.ToUpper(culture);
    }

    public static string ToLowerFirstLetter(this string value, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        if (value is null)
            throw new ArgumentNullException(nameof(value));
        else if (value.Length > 1)
            return char.ToLower(value[0], culture) + value[1..];
        else
            return value.ToLower(culture);
    }

    public static string[] SplitIntoWords(this string value)
    {
        ReadOnlySpan<char> chars = value.AsSpan();

        List<string> words = [];

        int wordStartIndex = char.IsLetterOrDigit(chars[0]) ? 0 : -1;

        static void EndWord(
            ref readonly ReadOnlySpan<char> source,
            ref int startIndex,
            ref readonly int endIndex,
            List<string> words)
        {
            if (startIndex != -1 && endIndex > startIndex)
            {
                words.Add(source[startIndex..endIndex].ToString());
                startIndex = -1;
            }
        }

        int i = 1;

        for (; i < chars.Length; i++)
        {
            char current = chars[i];
            char prev = chars[i - 1];
            char? next = i + 1 < chars.Length ? chars[i + 1] : null;

            if (!char.IsLetterOrDigit(current))
            {
                EndWord(ref chars, ref wordStartIndex, ref i, words);
            }
            else if ((char.IsDigit(current) && char.IsLetter(prev)) ||
                (char.IsLetter(current) && char.IsDigit(prev)) ||
                (char.IsUpper(current) && char.IsLower(prev)) ||
                (char.IsUpper(current) && next != null && char.IsLower(next.Value)))
            {
                EndWord(ref chars, ref wordStartIndex, ref i, words);
                wordStartIndex = i;
            }
            else if (wordStartIndex == -1)
            {
                wordStartIndex = i;
            }
        }

        EndWord(ref chars, ref wordStartIndex, ref i, words);

        return [.. words];
    }

    public static string Sanitize(this string value, char[] invalidChars)
    {
        Guard.ThrowIfNull(invalidChars);

        if (value is null or [])
            return value!;

        ReadOnlySpan<char> valueSpan = value.AsSpan();
        ReadOnlySpan<char> invalidCharsSpan = invalidChars;
        Span<char> resultSpan = valueSpan.Length <= 1024 ? stackalloc char[valueSpan.Length] : new char[valueSpan.Length];
        int resultSpanLength = 0;

        for (int i = 0; i < valueSpan.Length; i++)
        {
            if (invalidCharsSpan.IndexOf(valueSpan[i]) == -1)
                resultSpan[resultSpanLength++] = valueSpan[i];
        }

        return resultSpan[..resultSpanLength].ToString();
    }

    public static string Sanitize(this string value, char[] invalidChars, char replaceWith)
    {
        Guard.ThrowIfNull(invalidChars);

        if (value is null or [])
            return value!;

        ReadOnlySpan<char> valueSpan = value.AsSpan();
        ReadOnlySpan<char> invalidCharsSpan = invalidChars;
        Span<char> resultSpan = valueSpan.Length <= 1024 ? stackalloc char[valueSpan.Length] : new char[valueSpan.Length];

        for (int i = 0; i < valueSpan.Length; i++)
        {
            resultSpan[i] = invalidCharsSpan.IndexOf(valueSpan[i]) == -1
                ? valueSpan[i]
                : replaceWith;
        }

        return resultSpan.ToString();
    }

    public static string SanitizeForFileName(this string value)
    {
        s_invalidFileNameChars ??= Path.GetInvalidFileNameChars();
        return value.Sanitize(s_invalidFileNameChars);
    }

    public static string SanitizeForFileName(this string value, char replaceWith)
    {
        s_invalidFileNameChars ??= Path.GetInvalidFileNameChars();
        return value.Sanitize(s_invalidFileNameChars, replaceWith);
    }

    public static string SanitizeForPath(this string value)
    {
        s_invalidPathChars ??= Path.GetInvalidPathChars();
        return value.Sanitize(s_invalidPathChars);
    }

    public static string SanitizeForPath(this string value, char replaceWith)
    {
        s_invalidPathChars ??= Path.GetInvalidPathChars();
        return value.Sanitize(s_invalidPathChars, replaceWith);
    }

    public static string Truncate(this string value, int length, bool withEllipsis = true)
    {
        Guard.ThrowIfNull(value);

        const string ellipses = "...";
        Guard.ThrowIfLessThan(length, 1 + (withEllipsis ? ellipses.Length : 0));

        return value.Length <= length
            ? value
            : withEllipsis
                ? value[..(length - ellipses.Length)] + ellipses
                : value[..length];
    }

    public static string TrimStart(this string value, string trimString)
    {
        Guard.ThrowIfNull(value);

        return trimString is not null && value.StartsWith(trimString, StringComparison.Ordinal)
            ? value[trimString.Length..]
            : value;
    }
}
