namespace Atata;

public static partial class IObjectVerificationProviderExtensions
{
    private const StringComparison DefaultIgnoreCaseComparison = StringComparison.OrdinalIgnoreCase;

    public static TOwner BeNullOrEmpty<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier) =>
        verifier.Satisfy(string.IsNullOrEmpty, "be null or empty");

    public static TOwner BeNullOrWhiteSpace<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier) =>
        verifier.Satisfy(string.IsNullOrWhiteSpace, "be null or white-space");

    public static TOwner HaveLength<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, int expected) =>
        verifier.Satisfy(actual => actual is not null && actual.Length == expected, $"have length of {expected}");

    public static TOwner EqualIgnoringCase<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string expected) =>
        verifier.Satisfy(actual => string.Equals(expected, actual, DefaultIgnoreCaseComparison), "equal {0} ignoring case", expected);

    public static TOwner Contain<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string expected)
    {
        Guard.ThrowIfNull(expected);

        return verifier.Satisfy(
            actual => actual is not null && actual.IndexOf(expected, verifier.ResolveStringComparison()) != -1,
            VerificationMessage.Of("contain {0}", verifier.ResolveEqualityComparer<string>()),
            expected);
    }

    public static TOwner ContainIgnoringCase<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string expected)
    {
        Guard.ThrowIfNull(expected);

        return verifier.Satisfy(
            actual => actual is not null && actual.IndexOf(expected, DefaultIgnoreCaseComparison) != -1,
            "contain {0} ignoring case",
            expected);
    }

    public static TOwner StartWith<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string expected)
    {
        Guard.ThrowIfNull(expected);

        return verifier.Satisfy(
            actual => actual is not null && actual.StartsWith(expected, verifier.ResolveStringComparison()),
            VerificationMessage.Of("start with {0}", verifier.ResolveEqualityComparer<string>()),
            expected);
    }

    public static TOwner StartWithIgnoringCase<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string expected)
    {
        Guard.ThrowIfNull(expected);

        return verifier.Satisfy(
            actual => actual is not null && actual.StartsWith(expected, DefaultIgnoreCaseComparison),
            "start with {0} ignoring case",
            expected);
    }

    public static TOwner EndWith<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string expected)
    {
        Guard.ThrowIfNull(expected);

        return verifier.Satisfy(
            actual => actual is not null && actual.EndsWith(expected, verifier.ResolveStringComparison()),
            VerificationMessage.Of("end with {0}", verifier.ResolveEqualityComparer<string>()),
            expected);
    }

    public static TOwner EndWithIgnoringCase<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string expected)
    {
        Guard.ThrowIfNull(expected);

        return verifier.Satisfy(
            actual => actual is not null && actual.EndsWith(expected, DefaultIgnoreCaseComparison),
            "end with {0} ignoring case",
            expected);
    }

    [Obsolete("Use MatchRegex(...) instead.")] // Obsolete since v4.0.0.
    public static TOwner Match<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string pattern) =>
        verifier.MatchRegex(pattern, RegexOptions.None);

    [Obsolete("Use MatchRegex(...) instead.")] // Obsolete since v4.0.0.
    public static TOwner Match<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string pattern, RegexOptions regexOptions) =>
        verifier.MatchRegex(pattern, regexOptions);

    /// <summary>
    /// Verifies that a string matches the specified regular expression pattern.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner MatchRegex<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern) =>
        verifier.MatchRegex(pattern, RegexOptions.None);

    /// <inheritdoc cref="MatchRegex{TOwner}(IObjectVerificationProvider{string?, TOwner}, string)"/>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="pattern">The regular expression pattern to match.</param>
    /// <param name="options">The regular expression options.</param>
    public static TOwner MatchRegex<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, RegexOptions options)
    {
        Guard.ThrowIfNull(pattern);

        if (verifier.IsIgnoringCase())
            options |= RegexOptions.IgnoreCase;

        return verifier.Satisfy(
            actual => actual is not null && Regex.IsMatch(actual, pattern, options),
            $"match regex pattern \"{pattern}\"");
    }

    /// <summary>
    /// Verifies that a string matches the specified wildcard pattern.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="pattern">The wildcard pattern to match.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner MatchWildcardPattern<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, string pattern)
    {
        Guard.ThrowIfNull(pattern);

        StringComparison stringComparison = verifier.ResolveStringComparison();

        return verifier.Satisfy(
            actual => actual is not null && WildcardPattern.IsMatch(actual, pattern, stringComparison),
            $"match wildcard pattern \"{pattern}\"");
    }

    public static TOwner MatchAny<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, TermMatch match, params string[] expected)
    {
        Guard.ThrowIfNullOrEmpty(expected);

        var predicate = match.GetPredicate(verifier.ResolveStringComparison());

        string message = new StringBuilder()
            .Append($"{match.GetShouldText()} ")
            .AppendIf(expected.Length > 1, "any of: ")
            .AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}"))
            .ToString();

        return verifier.Satisfy(
            actual => actual is not null && expected.Any(x => predicate(actual, x)),
            VerificationMessage.Of(message, verifier.ResolveEqualityComparer<string>()),
            expected);
    }

    public static TOwner ContainAll<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, params string[] expected)
    {
        Guard.ThrowIfNullOrEmpty(expected);

        string message = new StringBuilder()
            .Append($"contain ")
            .AppendIf(expected.Length > 1, "all of: ")
            .AppendJoined(", ", Enumerable.Range(0, expected.Length).Select(x => $"{{{x}}}"))
            .ToString();

        StringComparison stringComparison = verifier.ResolveStringComparison();

        return verifier.Satisfy(
            actual => actual != null && expected.All(x => actual.IndexOf(x, stringComparison) != -1),
            VerificationMessage.Of(message, verifier.ResolveEqualityComparer<string>()),
            expected);
    }

    /// <inheritdoc cref="StartWithAny{TOwner}(IObjectVerificationProvider{string?, TOwner}, IEnumerable{string})"/>
    public static TOwner StartWithAny<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, params string[] expected) =>
        verifier.StartWithAny(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that a string starts with any of the expected strings.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">The expected values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner StartWithAny<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, IEnumerable<string> expected)
    {
        Guard.ThrowIfNullOrEmpty(expected);

        StringComparison stringComparison = verifier.ResolveStringComparison();

        return verifier.Satisfy(
            actual => actual is not null && expected.Any(x => actual.StartsWith(x, stringComparison)),
            VerificationMessage.Of($"start with any of {Stringifier.ToString(expected)}", verifier.ResolveEqualityComparer<string>()));
    }

    /// <inheritdoc cref="EndWithAny{TOwner}(IObjectVerificationProvider{string?, TOwner}, IEnumerable{string})"/>
    public static TOwner EndWithAny<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, params string[] expected) =>
        verifier.EndWithAny(expected?.AsEnumerable()!);

    /// <summary>
    /// Verifies that a string ends with any of the expected strings.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    /// <param name="verifier">The verification provider.</param>
    /// <param name="expected">The expected values.</param>
    /// <returns>The owner instance.</returns>
    public static TOwner EndWithAny<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier, IEnumerable<string> expected)
    {
        Guard.ThrowIfNullOrEmpty(expected);

        StringComparison stringComparison = verifier.ResolveStringComparison();

        return verifier.Satisfy(
            actual => actual is not null && expected.Any(x => actual.EndsWith(x, stringComparison)),
            VerificationMessage.Of($"end with any of {Stringifier.ToString(expected)}", verifier.ResolveEqualityComparer<string>()));
    }

    private static bool IsIgnoringCase<TOwner>(this IObjectVerificationProvider<string?, TOwner> verifier) =>
        verifier.ResolveStringComparison()
            is StringComparison.CurrentCultureIgnoreCase
            or StringComparison.InvariantCultureIgnoreCase
            or StringComparison.OrdinalIgnoreCase;
}
