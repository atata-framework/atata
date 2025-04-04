namespace Atata;

public class TermOptions : ITermSettings, IHasOptionalProperties
{
    public PropertyBag OptionalProperties { get; } = new();

    /// <summary>
    /// Gets or sets the match.
    /// </summary>
    public TermMatch Match
    {
        get => OptionalProperties.GetOrDefault(nameof(Match), TermMatch.Equals);
        set => OptionalProperties[nameof(Match)] = value;
    }

    /// <summary>
    /// Gets or sets the term case.
    /// </summary>
    public TermCase Case
    {
        get => OptionalProperties.GetOrDefault(nameof(Case), TermCase.None);
        set => OptionalProperties[nameof(Case)] = value;
    }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    public string? Format
    {
        get => OptionalProperties.GetOrDefault<string>(nameof(Format));
        set => OptionalProperties[nameof(Format)] = value;
    }

    /// <summary>
    /// Gets or sets the culture.
    /// </summary>
    public CultureInfo Culture
    {
        get => OptionalProperties.GetOrDefault(nameof(Culture), CultureInfo.CurrentCulture) ?? CultureInfo.CurrentCulture;
        set => OptionalProperties[nameof(Culture)] = value;
    }

    public TermOptions MergeWith(IHasOptionalProperties settingsAttribute)
    {
        settingsAttribute.CheckNotNull(nameof(settingsAttribute));

        if (settingsAttribute.OptionalProperties.TryGet(nameof(Case), out TermCase termCase))
            Case = termCase;

        if (settingsAttribute.OptionalProperties.TryGet(nameof(Match), out TermMatch termMatch))
            Match = termMatch;

        if (settingsAttribute.OptionalProperties.TryGet(nameof(Format), out string? format))
            Format = format;

        if (settingsAttribute.OptionalProperties.TryGet(nameof(Culture), out CultureInfo? culture))
            Culture = culture!;

        return this;
    }

    public TermOptions WithFormat(string format)
    {
        Format = format;
        return this;
    }
}
