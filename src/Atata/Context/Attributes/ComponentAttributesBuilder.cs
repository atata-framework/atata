namespace Atata;

/// <summary>
/// Represents the builder of component attributes.
/// </summary>
public sealed class ComponentAttributesBuilder
    : AttributesBuilder<ComponentAttributesBuilder>
{
    private readonly Type _componentType;

    private readonly AtataAttributesContext _attributesContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentAttributesBuilder"/> class.
    /// </summary>
    /// <param name="componentType">Type of the component.</param>
    /// <param name="attributesContext">The building attributes context.</param>
    public ComponentAttributesBuilder(
        Type componentType,
        AtataAttributesContext attributesContext)
    {
        _componentType = componentType;
        _attributesContext = attributesContext;
    }

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesBuilder<ComponentAttributesBuilder> this[string propertyName] =>
        Property(propertyName);

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesBuilder<ComponentAttributesBuilder> Property(string propertyName)
    {
        propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

        return new PropertyAttributesBuilder<ComponentAttributesBuilder>(
            _componentType,
            propertyName,
            this,
            _attributesContext);
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

    protected override ComponentAttributesBuilder ResolveNextBuilder() => this;
}
