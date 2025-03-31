#nullable enable

namespace Atata;

/// <summary>
/// Represents the base attribute class for the finding attributes that use terms.
/// </summary>
public abstract class TermFindAttribute : FindAttribute, ITermFindAttribute, ITermMatchFindAttribute, ITermSettings
{
    protected TermFindAttribute(TermCase termCase)
        : this() =>
        Case = termCase;

    protected TermFindAttribute(TermMatch match, TermCase termCase)
        : this()
    {
        Match = match;
        Case = termCase;
    }

    protected TermFindAttribute(TermMatch match, params string[] values)
        : this(values) =>
        Match = match;

    protected TermFindAttribute(params string[] values)
    {
        if (values?.Length > 0)
            Values = values;
    }

    /// <summary>
    /// Gets the term values.
    /// </summary>
    public string[]? Values
    {
        get => ResolveValues();
        private set => OptionalProperties[nameof(Values)] = value;
    }

    /// <summary>
    /// Gets the term case.
    /// </summary>
    public TermCase Case
    {
        get => ResolveCase();
        private set => OptionalProperties[nameof(Case)] = value;
    }

    /// <summary>
    /// Gets the match.
    /// </summary>
    public new TermMatch Match
    {
        get => ResolveMatch();
        private set => OptionalProperties[nameof(Match)] = value;
    }

    /// <summary>
    /// Gets or sets the format.
    /// </summary>
    public string? Format
    {
        get => ResolveFormat();
        set => OptionalProperties[nameof(Format)] = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the name should be cut
    /// considering the <see cref="UIComponentDefinitionAttribute.IgnoreNameEndings"/> property value
    /// of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>.
    /// The default value is <see langword="true"/>.
    /// </summary>
    public bool CutEnding
    {
        get => ResolveCutEnding();
        set => OptionalProperties[nameof(CutEnding)] = value;
    }

    /// <summary>
    /// Gets the default term case.
    /// </summary>
    protected abstract TermCase DefaultCase { get; }

    /// <summary>
    /// Gets the default match.
    /// The default value is <see cref="TermMatch.Equals"/>.
    /// </summary>
    protected virtual TermMatch DefaultMatch => TermMatch.Equals;

    internal string[]? ResolveValues(UIComponentMetadata? metadata = null) =>
        OptionalProperties.Resolve<string[]>(
            nameof(Values),
            metadata != null ? GetTermAttributes(metadata) : null);

    internal TermCase ResolveCase(UIComponentMetadata? metadata = null) =>
        OptionalProperties.Resolve(
            nameof(Case),
            DefaultCase,
            metadata != null ? GetTermAndTermFindSettingsAttributes(metadata) : null);

    internal TermMatch ResolveMatch(UIComponentMetadata? metadata = null) =>
        OptionalProperties.Resolve(
            nameof(Match),
            DefaultMatch,
            metadata != null ? GetTermAndTermFindSettingsAttributes(metadata) : null);

    internal string? ResolveFormat(UIComponentMetadata? metadata = null) =>
        OptionalProperties.Resolve<string>(
            nameof(Format),
            metadata != null ? GetTermAndTermFindSettingsAttributes(metadata) : null);

    internal bool ResolveCutEnding(UIComponentMetadata? metadata = null) =>
        OptionalProperties.Resolve(
            nameof(CutEnding),
            true,
            metadata != null ? GetTermAndTermFindSettingsAttributes(metadata) : null);

    public string[] GetTerms(UIComponentMetadata metadata)
    {
        string[] rawTerms = GetRawTerms(metadata);
        string? format = ResolveFormat(metadata);

        return format?.Length > 0
            ? [.. rawTerms.Select(x => string.Format(format, x))]
            : rawTerms;
    }

    protected virtual string[] GetRawTerms(UIComponentMetadata metadata) =>
        ResolveValues(metadata)
            ?? [GetTermFromProperty(metadata)];

    private string GetTermFromProperty(UIComponentMetadata metadata)
    {
        string name = GetPropertyName(metadata);
        return ResolveCase(metadata).ApplyTo(name);
    }

    public TermMatch GetTermMatch(UIComponentMetadata metadata) =>
        ResolveMatch(metadata);

    private string GetPropertyName(UIComponentMetadata metadata) =>
        ResolveCutEnding(metadata)
            ? metadata.ComponentDefinitionAttribute.NormalizeNameIgnoringEnding(metadata.Name!)
            : metadata.Name!;

    public override string BuildComponentName(UIComponentMetadata metadata)
    {
        var values = ResolveValues(metadata);

        return values?.Length > 0
            ? BuildComponentNameWithArgument(string.Join("/", values))
            : throw new InvalidOperationException($"Component name cannot be resolved automatically for {GetType().Name}. Term value(s) should be specified explicitly.");
    }

    private static IEnumerable<IHasOptionalProperties> GetTermAttributes(UIComponentMetadata metadata) =>
        metadata.GetAll<TermAttribute>();

    private IEnumerable<IHasOptionalProperties> GetTermFindSettingAttributes(UIComponentMetadata metadata) =>
        metadata.GetAll<TermFindSettingsAttribute>(x => x.ForAttribute(GetType()));

    private IEnumerable<IHasOptionalProperties> GetTermAndTermFindSettingsAttributes(UIComponentMetadata metadata) =>
        GetTermAttributes(metadata).Concat(GetTermFindSettingAttributes(metadata));
}
