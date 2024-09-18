namespace Atata;

/// <summary>
/// Represents the builder of global level attributes.
/// </summary>
public sealed class GlobalAttributesBuilder : AttributesBuilder<GlobalAttributesBuilder>
{
    private readonly AtataAttributesContext _attributesContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalAttributesBuilder"/> class.
    /// </summary>
    /// <param name="attributesContext">The building attributes context.</param>
    public GlobalAttributesBuilder(AtataAttributesContext attributesContext) =>
        _attributesContext = attributesContext;

    protected override void OnAdd(IEnumerable<Attribute> attributes) =>
        _attributesContext.Global.AddRange(attributes);

    protected override GlobalAttributesBuilder ResolveNextBuilder() => this;
}
