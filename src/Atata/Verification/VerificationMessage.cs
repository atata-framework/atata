namespace Atata;

public sealed class VerificationMessage
{
    private readonly StringBuilder _stringBuilder;

    public VerificationMessage(string text) =>
        _stringBuilder = new(text);

    [return: NotNullIfNotNull(nameof(message))]
    public static implicit operator string?(VerificationMessage? message) =>
        message?.ToString();

    public static VerificationMessage Of<T>(string text, IEqualityComparer<T> equalityComparer) =>
        new VerificationMessage(text)
            .With(equalityComparer);

    public VerificationMessage Append(string text)
    {
        _stringBuilder.Append(text);
        return this;
    }

    public VerificationMessage With<T>(IEqualityComparer<T> equalityComparer)
    {
        string? equalityComparerDescription = ResolveEqualityComparerDescription(equalityComparer);

        if (equalityComparerDescription?.Length > 0)
            _stringBuilder.Append(' ').Append(equalityComparerDescription);

        return this;
    }

    private static string? ResolveEqualityComparerDescription<T>(IEqualityComparer<T> equalityComparer)
    {
        if (equalityComparer is StringComparer)
        {
            if (equalityComparer == StringComparer.CurrentCultureIgnoreCase ||
                equalityComparer == StringComparer.InvariantCultureIgnoreCase ||
                equalityComparer == StringComparer.OrdinalIgnoreCase)
                return "ignoring case";
        }
        else if (equalityComparer is IDescribesComparison describer)
        {
            return describer.GetComparisonDescription();
        }

        return null;
    }

    public override string ToString() =>
        _stringBuilder.ToString();
}
