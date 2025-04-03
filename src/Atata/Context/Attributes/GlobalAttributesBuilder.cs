#nullable enable

namespace Atata;

/// <summary>
/// A builder of global level attributes.
/// </summary>
public sealed class GlobalAttributesBuilder : AttributesBuilderBase
{
    private readonly AtataAttributesContext _attributesContext;

    internal GlobalAttributesBuilder(
        AtataContextBuilder atataContextBuilder,
        AtataAttributesContext attributesContext)
        : base(atataContextBuilder)
        =>
        _attributesContext = attributesContext;

    protected override void OnAdd(IEnumerable<Attribute> attributes) =>
        _attributesContext.Global.AddRange(attributes);
}
