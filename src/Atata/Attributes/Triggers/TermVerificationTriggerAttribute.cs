namespace Atata;

/// <summary>
/// The base trigger attribute class that can be used in the verification process when the page object is initialized.
/// </summary>
public abstract class TermVerificationTriggerAttribute : WaitingTriggerAttribute, ITermSettings, IHasOptionalProperties
{
    protected TermVerificationTriggerAttribute(TermCase termCase)
        : this() =>
        Case = termCase;

    protected TermVerificationTriggerAttribute(TermMatch match, TermCase termCase)
        : this()
    {
        Match = match;
        Case = termCase;
    }

    protected TermVerificationTriggerAttribute(TermMatch match, params string[] values)
        : this(values) =>
        Match = match;

    protected TermVerificationTriggerAttribute(params string[] values)
        : base(TriggerEvents.Init) =>
        Values = values;

    PropertyBag IHasOptionalProperties.OptionalProperties => OptionalProperties;

    protected PropertyBag OptionalProperties { get; } = new PropertyBag();

    public string[] Values { get; }

    public TermCase Case
    {
        get => ResolveCase();
        private set => OptionalProperties[nameof(Case)] = value;
    }

    protected virtual TermCase DefaultCase => TermCase.Title;

    public new TermMatch Match
    {
        get => ResolveMatch();
        private set => OptionalProperties[nameof(Match)] = value;
    }

    protected virtual TermMatch DefaultMatch => TermMatch.Equals;

    public string Format
    {
        get => ResolveFormat();
        set => OptionalProperties[nameof(Format)] = value;
    }

    internal TermCase ResolveCase(UIComponentMetadata metadata = null) =>
        OptionalProperties.Resolve(
            nameof(Case),
            DefaultCase,
            metadata != null ? GetSettingsAttributes(metadata) : null);

    internal TermMatch ResolveMatch(UIComponentMetadata metadata = null) =>
        OptionalProperties.Resolve(
            nameof(Match),
            DefaultMatch,
            metadata != null ? GetSettingsAttributes(metadata) : null);

    internal string ResolveFormat(UIComponentMetadata metadata = null) =>
        OptionalProperties.Resolve<string>(
            nameof(Format),
            metadata != null ? GetSettingsAttributes(metadata) : null);

    protected virtual IEnumerable<IHasOptionalProperties> GetSettingsAttributes(UIComponentMetadata metadata) =>
        Enumerable.Empty<IHasOptionalProperties>();

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        string[] actualValues = ResolveActualValues(context.Component.Metadata, context.Component.ComponentName);

        OnExecute(context, actualValues);
    }

    protected abstract void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        where TOwner : PageObject<TOwner>;

    private string[] ResolveActualValues(UIComponentMetadata metadata, string fallbackValue)
    {
        string[] rawValues = Values?.Any() ?? false
            ? Values
            : [ResolveCase(metadata).ApplyTo(fallbackValue ?? throw new ArgumentNullException(nameof(fallbackValue)))];

        string format = ResolveFormat(metadata);

        return !string.IsNullOrEmpty(format)
            ? rawValues.Select(x => string.Format(format, x)).ToArray()
            : rawValues;
    }
}
