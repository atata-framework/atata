namespace Atata;

/// <summary>
/// A builder of assembly attributes.
/// </summary>
public sealed class AssemblyAttributesBuilder : AttributesBuilderBase
{
    private readonly AtataAttributesContext _attributesContext;

    private readonly Assembly _assembly;

    internal AssemblyAttributesBuilder(
        AtataContextBuilder atataContextBuilder,
        AtataAttributesContext attributesContext,
        Assembly assembly)
        : base(atataContextBuilder)
    {
        _attributesContext = attributesContext;
        _assembly = assembly;
    }

    protected override void OnAdd(IEnumerable<Attribute> attributes)
    {
        if (!_attributesContext.AssemblyMap.TryGetValue(_assembly, out var attributeSet))
        {
            attributeSet = [];
            _attributesContext.AssemblyMap[_assembly] = attributeSet;
        }

        attributeSet.AddRange(attributes);
    }
}
