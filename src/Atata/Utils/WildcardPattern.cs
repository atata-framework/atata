namespace Atata;

/// <summary>
/// Represents a wildcard pattern.
/// Wildcards: <c>?</c> matches exactly one occurrence of any character;
/// <c>*</c> matches arbitrary many (including zero) occurrences of any character.
/// To escape <c>?</c> or <c>*</c> prepend it with <c>\</c> character.
/// </summary>
public class WildcardPattern
{
    private readonly string _pattern;

    private readonly StringComparison _stringComparison;

    /// <summary>
    /// Initializes a new instance of the <see cref="WildcardPattern"/> class.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    public WildcardPattern(string pattern)
        : this(pattern, StringComparison.Ordinal)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="WildcardPattern"/> class.
    /// </summary>
    /// <param name="pattern">The pattern.</param>
    /// <param name="stringComparison">The string comparison type.</param>
    public WildcardPattern(string pattern, StringComparison stringComparison)
    {
        Guard.ThrowIfNull(pattern);

        _pattern = pattern;
        _stringComparison = stringComparison;
    }

    /// <inheritdoc cref="IsMatch(string)"/>
    /// <param name="input">The text to search for a match.</param>
    /// <param name="pattern">The pattern.</param>
    public static bool IsMatch(string input, string pattern) =>
        IsMatch(Guard.ReturnOrThrowIfNull(input).AsSpan(), pattern, StringComparison.Ordinal);

    /// <inheritdoc cref="IsMatch(string, string)"/>
    public static bool IsMatch(ReadOnlySpan<char> input, string pattern) =>
        IsMatch(input, pattern, StringComparison.Ordinal);

    /// <inheritdoc cref="IsMatch(string)"/>
    /// <param name="input">The text to search for a match.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="stringComparison">The string comparison type.</param>
    public static bool IsMatch(string input, string pattern, StringComparison stringComparison) =>
        IsMatch(Guard.ReturnOrThrowIfNull(input).AsSpan(), pattern, stringComparison);

    /// <inheritdoc cref="IsMatch(string, string, StringComparison)"/>
    public static bool IsMatch(ReadOnlySpan<char> input, string pattern, StringComparison stringComparison) =>
        new WildcardPattern(pattern, stringComparison).IsMatch(input);

    /// <summary>
    /// Determines whether the specified <paramref name="input"/> matches the pattern.
    /// </summary>
    /// <param name="input">The text to search for a match.</param>
    /// <returns>
    /// <see langword="true"/> if the specified input matches the pattern; otherwise, <see langword="false"/>.
    /// </returns>
    public bool IsMatch(string input) =>
        IsMatch(Guard.ReturnOrThrowIfNull(input).AsSpan());

    /// <inheritdoc cref="IsMatch(string)"/>
    public bool IsMatch(ReadOnlySpan<char> input)
    {
        ReadOnlySpan<char> patternSpan = _pattern.AsSpan();

        int inputIndex = 0;
        int patternIndex = 0;
        int patternLength = patternSpan.Length;
        int patternStarIndex = -1;
        int inputOnStarIndex = -1;

        while (inputIndex < input.Length)
        {
            if (patternIndex + 1 < patternLength
                && patternSpan[patternIndex] == '\\'
                && patternSpan[patternIndex + 1] is '*' or '?'
                && input[inputIndex] == patternSpan[patternIndex + 1])
            {
                inputIndex++;
                patternIndex += 2;
            }
            else if (patternIndex < patternLength
                && (patternSpan[patternIndex] == '?' || input.Slice(inputIndex, 1).Equals(patternSpan.Slice(patternIndex, 1), _stringComparison)))
            {
                inputIndex++;
                patternIndex++;
            }
            else if (patternIndex < patternLength && patternSpan[patternIndex] == '*')
            {
                patternStarIndex = patternIndex;
                inputOnStarIndex = inputIndex;
                patternIndex++;
            }
            else if (patternStarIndex != -1)
            {
                patternIndex = patternStarIndex + 1;
                inputOnStarIndex++;
                inputIndex = inputOnStarIndex;
            }
            else
            {
                return false;
            }
        }

        while (patternIndex < patternLength && patternSpan[patternIndex] == '*')
        {
            patternIndex++;
        }

        return patternIndex == patternLength;
    }
}
