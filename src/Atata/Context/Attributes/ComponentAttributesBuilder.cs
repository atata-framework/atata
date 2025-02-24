namespace Atata;

/// <summary>
/// A builder of component attributes.
/// </summary>
public sealed class ComponentAttributesBuilder : AttributesBuilderBase
{
    private readonly AtataAttributesContext _attributesContext;

    private readonly Type _componentType;

    internal ComponentAttributesBuilder(
        AtataContextBuilder atataContextBuilder,
        AtataAttributesContext attributesContext,
        Type componentType)
        : base(atataContextBuilder)
    {
        _attributesContext = attributesContext;
        _componentType = componentType;
    }

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder"/>.</returns>
    public PropertyAttributesBuilder this[string propertyName] =>
        Property(propertyName);

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder"/>.</returns>
    public PropertyAttributesBuilder Property(string propertyName)
    {
        propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

        return new(
            AtataContextBuilder,
            _attributesContext,
            _componentType,
            propertyName);
    }

    protected override void OnAdd(IEnumerable<Attribute> attributes)
    {
        if (!_attributesContext.ComponentMap.TryGetValue(_componentType, out var attributeSet))
        {
            attributeSet = [];
            _attributesContext.ComponentMap[_componentType] = attributeSet;
        }

        attributeSet.AddRange(attributes);
    }
}
