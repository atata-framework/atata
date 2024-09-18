namespace Atata;

/// <summary>
/// Represents the builder of property attributes.
/// </summary>
/// <typeparam name="TNextBuilder">The type of the next builder to return by <c>Add</c> methods.</typeparam>
public sealed class PropertyAttributesBuilder<TNextBuilder>
    : AttributesBuilder<TNextBuilder>
{
    private readonly TypePropertyNamePair _typeProperty;

    private readonly TNextBuilder _parentBuilder;

    private readonly AtataAttributesContext _attributesContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyAttributesBuilder{TNextBuilder}"/> class.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <param name="parentBuilder">The parent builder.</param>
    /// <param name="attributesContext">The building attributes context.</param>
    public PropertyAttributesBuilder(
        Type type,
        string propertyName,
        TNextBuilder parentBuilder,
        AtataAttributesContext attributesContext)
    {
        _typeProperty = new TypePropertyNamePair(type, propertyName);
        _parentBuilder = parentBuilder;
        _attributesContext = attributesContext;
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

    protected override TNextBuilder ResolveNextBuilder() => _parentBuilder;
}
