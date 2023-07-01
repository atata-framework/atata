namespace Atata;

/// <summary>
/// Represents the builder of component attributes.
/// </summary>
public class ComponentAttributesAtataContextBuilder
    : AttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder>
{
    private readonly Type _componentType;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentAttributesAtataContextBuilder"/> class.
    /// </summary>
    /// <param name="componentType">Type of the component.</param>
    /// <param name="buildingContext">The building context.</param>
    public ComponentAttributesAtataContextBuilder(Type componentType, AtataBuildingContext buildingContext)
        : base(buildingContext) =>
        _componentType = componentType;

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesAtataContextBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder> this[string propertyName] =>
        Property(propertyName);

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesAtataContextBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder> Property(string propertyName)
    {
        propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

        return new PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder>(
            _componentType,
            propertyName,
            this,
            BuildingContext);
    }

    protected override void OnAdd(IEnumerable<Attribute> attributes)
    {
        if (!BuildingContext.Attributes.ComponentMap.TryGetValue(_componentType, out var attributeSet))
        {
            attributeSet = new List<Attribute>();
            BuildingContext.Attributes.ComponentMap[_componentType] = attributeSet;
        }

        attributeSet.AddRange(attributes);
    }

    protected override ComponentAttributesAtataContextBuilder ResolveNextBuilder() => this;
}
