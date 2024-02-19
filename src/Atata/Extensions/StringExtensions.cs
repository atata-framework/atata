namespace Atata;

public static class StringExtensions
{
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

    public static string ToUpperFirstLetter(this string value)
    {
        if (value == null)
            return null;
        else if (value.Length > 1)
            return char.ToUpper(value[0], CultureInfo.CurrentCulture) + value.Substring(1);
        else
            return value.ToUpper(CultureInfo.CurrentCulture);
    }

    public static string ToLowerFirstLetter(this string value)
    {
        if (value == null)
            return null;
        else if (value.Length > 1)
            return char.ToLower(value[0], CultureInfo.CurrentCulture) + value.Substring(1);
        else
            return value.ToLower(CultureInfo.CurrentCulture);
    }

    public static string PascalDasherize(this string underscoredWord)
    {
        string[] parts = underscoredWord.Split('-');
        return string.Join("-", parts.Select(x => x.ToUpperFirstLetter()));
    }

    public static string PascalHyphenate(this string underscoredWord)
    {
        string[] parts = underscoredWord.Split('_');
        return string.Join("‐", parts.Select(x => x.ToUpperFirstLetter()));
    }

    public static string[] SplitIntoWords(this string value)
    {
        char[] chars = value.ToCharArray();

        List<char> wordChars = [];
        List<string> words = [];

        if (char.IsLetterOrDigit(chars[0]))
            wordChars.Add(chars[0]);

        void EndWord()
        {
            if (wordChars.Any())
            {
                words.Add(new string([.. wordChars]));
                wordChars.Clear();
            }
        }

        for (int i = 1; i < chars.Length; i++)
        {
            char current = chars[i];
            char prev = chars[i - 1];
            char? next = i + 1 < chars.Length ? (char?)chars[i + 1] : null;

            if (!char.IsLetterOrDigit(current))
            {
                EndWord();
            }
            else if ((char.IsDigit(current) && char.IsLetter(prev)) ||
                (char.IsLetter(current) && char.IsDigit(prev)) ||
                (char.IsUpper(current) && char.IsLower(prev)) ||
                (char.IsUpper(current) && next != null && char.IsLower(next.Value)))
            {
                EndWord();
                wordChars.Add(current);
            }
            else
            {
                wordChars.Add(current);
            }
        }

        EndWord();

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

    public static string SanitizeForFileName(this string value, string replaceWith = null) =>
        value.Sanitize(Path.GetInvalidFileNameChars(), replaceWith);

    public static string SanitizeForFileName(this string value, char replaceWith) =>
        value.Sanitize(Path.GetInvalidFileNameChars(), replaceWith);

    public static string SanitizeForPath(this string value, string replaceWith = null) =>
        value.Sanitize(Path.GetInvalidPathChars(), replaceWith);

    public static string SanitizeForPath(this string value, char replaceWith) =>
        value.Sanitize(Path.GetInvalidPathChars(), replaceWith);

    public static string Truncate(this string value, int length, bool withEllipsis = true)
    {
        value.CheckNotNull(nameof(value));

        const string ellipses = "...";
        length.CheckGreaterOrEqual(nameof(length), 1 + (withEllipsis ? ellipses.Length : 0));

        return value.Length <= length
            ? value
            : withEllipsis
            ? value.Substring(0, length - ellipses.Length) + ellipses
            : value.Substring(0, length);
    }

    public static string TrimStart(this string value, string trimString)
    {
        value.CheckNotNull(nameof(value));

        return trimString is not null && value.StartsWith(trimString, StringComparison.Ordinal)
            ? value.Substring(trimString.Length)
            : value;
    }
}
