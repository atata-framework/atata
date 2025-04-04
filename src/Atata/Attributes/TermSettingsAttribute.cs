namespace Atata;

/// <summary>
/// Specifies the term settings.
/// </summary>
public class TermSettingsAttribute : MulticastAttribute, ITermSettings, IHasOptionalProperties
{
    public TermSettingsAttribute()
    {
    }

    public TermSettingsAttribute(TermCase termCase) =>
        Case = termCase;

    public TermSettingsAttribute(TermMatch match) =>
        Match = match;

    public TermSettingsAttribute(TermMatch match, TermCase termCase)
    {
        Match = match;
        Case = termCase;
    }

    PropertyBag IHasOptionalProperties.OptionalProperties => OptionalProperties;

    protected PropertyBag OptionalProperties { get; } = new();

    /// <summary>
    /// Gets the match.
    /// </summary>
    public new TermMatch Match
    {
        get => OptionalProperties.GetOrDefault(nameof(Match), TermMatch.Equals);
        private set => OptionalProperties[nameof(Match)] = value;
    }

    /// <summary>
    /// Gets the term case.
    /// </summary>
    public TermCase Case
    {
        get => OptionalProperties.GetOrDefault(nameof(Case), TermCase.None);
        private set => OptionalProperties[nameof(Case)] = value;
    }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    public string? Format
    {
        get => OptionalProperties.GetOrDefault<string>(nameof(Format));
        set => OptionalProperties[nameof(Format)] = value;
    }
}
