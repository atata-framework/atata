namespace Atata;

public static class StringExtensions
{
    private static char[] s_invalidFileNameChars;

    private static char[] s_invalidPathChars;

    internal static string FormatWith(this string format, params object[] args) =>
        string.Format(format, args);

    public static string Prepend(this string value, string valueToPrepend) =>
        string.Concat(valueToPrepend, value);

    public static string Append(this string value, string valueToAppend) =>
        string.Concat(value, valueToAppend);

    public static bool IsUpper(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return false;

        return value.ToCharArray().All(char.IsUpper);
    }

    public static string ToUpperFirstLetter(this string value, CultureInfo culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        if (value == null)
            return null;
        else if (value.Length > 1)
            return char.ToUpper(value[0], culture) + value[1..];
        else
            return value.ToUpper(culture);
    }

    public static string ToLowerFirstLetter(this string value, CultureInfo culture = null)
    {
        culture ??= CultureInfo.CurrentCulture;

        if (value == null)
            return null;
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

    public static string Sanitize(this string value, IEnumerable<char> invalidChars, string replaceWith = null)
    {
        invalidChars.CheckNotNull(nameof(invalidChars));

        if (string.IsNullOrEmpty(value))
            return value;

        return invalidChars.Aggregate(value, (current, c) => current.Replace(c.ToString(), replaceWith));
    }

    public static string Sanitize(this string value, IEnumerable<char> invalidChars, char replaceWith)
    {
        invalidChars.CheckNotNull(nameof(invalidChars));

        if (string.IsNullOrEmpty(value))
            return value;

        return invalidChars.Aggregate(value, (current, c) => current.Replace(c, replaceWith));
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
        value.CheckNotNull(nameof(value));

        const string ellipses = "...";
        length.CheckGreaterOrEqual(nameof(length), 1 + (withEllipsis ? ellipses.Length : 0));

        return value.Length <= length
            ? value
            : withEllipsis
                ? value[..(length - ellipses.Length)] + ellipses
                : value[..length];
    }

    public static string TrimStart(this string value, string trimString)
    {
        value.CheckNotNull(nameof(value));

        return trimString is not null && value.StartsWith(trimString, StringComparison.Ordinal)
            ? value[trimString.Length..]
            : value;
    }
}
