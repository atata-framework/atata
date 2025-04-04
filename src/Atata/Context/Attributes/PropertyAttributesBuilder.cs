namespace Atata;

/// <summary>
/// A builder of property attributes.
/// </summary>
public sealed class PropertyAttributesBuilder : AttributesBuilderBase
{
    private readonly AtataAttributesContext _attributesContext;

    private readonly TypePropertyNamePair _typeProperty;

    internal PropertyAttributesBuilder(
        AtataContextBuilder atataContextBuilder,
        AtataAttributesContext attributesContext,
        Type type,
        string propertyName)
        : base(atataContextBuilder)
    {
        _attributesContext = attributesContext;
        _typeProperty = new(type, propertyName);
    }

    protected override void OnAdd(IEnumerable<Attribute> attributes)
    {
        if (!_attributesContext.PropertyMap.TryGetValue(_typeProperty, out var attributeSet))
        {
            attributeSet = [];
            _attributesContext.PropertyMap[_typeProperty] = attributeSet;
        }

        attributeSet.AddRange(attributes);
    }
}
