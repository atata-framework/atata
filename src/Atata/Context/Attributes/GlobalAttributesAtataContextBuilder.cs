namespace Atata;

/// <summary>
/// Represents the builder of global level attributes.
/// </summary>
public sealed class GlobalAttributesAtataContextBuilder : AttributesAtataContextBuilder<GlobalAttributesAtataContextBuilder>
{
    private readonly AtataAttributesContext _attributesContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalAttributesAtataContextBuilder"/> class.
    /// </summary>
    /// <param name="attributesContext">The building attributes context.</param>
    public GlobalAttributesAtataContextBuilder(AtataAttributesContext attributesContext) =>
        _attributesContext = attributesContext;

    protected override void OnAdd(IEnumerable<Attribute> attributes) =>
        _attributesContext.Global.AddRange(attributes);

    protected override GlobalAttributesAtataContextBuilder ResolveNextBuilder() => this;
}
