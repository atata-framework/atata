namespace Atata;

public interface ITermSettings
{
    /// <summary>
    /// Gets the term case.
    /// </summary>
    TermCase Case { get; }

    /// <summary>
    /// Gets the match.
    /// </summary>
    TermMatch Match { get; }

    /// <summary>
    /// Gets the format.
    /// </summary>
    string? Format { get; }
}
