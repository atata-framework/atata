namespace Atata;

/// <summary>
/// Defines the term settings to apply for the specified finding strategy of a control.
/// Adds to or overrides properties of <see cref="TermFindAttribute"/>.
/// </summary>
public class TermFindSettingsAttribute : FindSettingsAttribute, ITermSettings
{
    /// <summary>
    /// Gets or sets the term case.
    /// </summary>
    public TermCase Case
    {
        get => OptionalProperties.GetOrDefault(nameof(Case), TermCase.None);
        set => OptionalProperties[nameof(Case)] = value;
    }

    /// <summary>
    /// Gets or sets the match.
    /// </summary>
    public new TermMatch Match
    {
        get => OptionalProperties.GetOrDefault(nameof(Match), TermMatch.Equals);
        set => OptionalProperties[nameof(Match)] = value;
    }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    public string Format
    {
        get => OptionalProperties.GetOrDefault<string>(nameof(Format));
        set => OptionalProperties[nameof(Format)] = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the name should be cut
    /// considering the <c>IgnoreNameEndings</c> property value of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool CutEnding
    {
        get => OptionalProperties.GetOrDefault(nameof(CutEnding), true);
        set => OptionalProperties[nameof(CutEnding)] = value;
    }
}
