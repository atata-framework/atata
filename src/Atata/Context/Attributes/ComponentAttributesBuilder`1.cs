namespace Atata;

/// <summary>
/// Represents the builder of <typeparamref name="TComponent"/> component attributes.
/// </summary>
/// <typeparam name="TComponent">The type of the component.</typeparam>
public sealed class ComponentAttributesBuilder<TComponent>
    : AttributesBuilder<ComponentAttributesBuilder<TComponent>>
{
    private readonly Type _componentType;

    private readonly AtataAttributesContext _attributesContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ComponentAttributesBuilder{TComponent}"/> class.
    /// </summary>
    /// <param name="attributesContext">The building attributes context.</param>
    public ComponentAttributesBuilder(AtataAttributesContext attributesContext)
    {
        _componentType = typeof(TComponent);
        _attributesContext = attributesContext;
    }

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesBuilder<ComponentAttributesBuilder<TComponent>> this[string propertyName] =>
        Property(propertyName);

    /// <summary>
    /// Creates and returns the attributes builder for the property specified by expression.
    /// </summary>
    /// <param name="propertyExpression">The expression returning the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesBuilder<ComponentAttributesBuilder<TComponent>> this[Expression<Func<TComponent, object>> propertyExpression] =>
        Property(propertyExpression);

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesBuilder<ComponentAttributesBuilder<TComponent>> Property(string propertyName)
    {
        propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

        return new PropertyAttributesBuilder<ComponentAttributesBuilder<TComponent>>(
            _componentType,
            propertyName,
            this,
            _attributesContext);
    }

    /// <summary>
    /// Creates and returns the attributes builder for the property specified by expression.
    /// </summary>
    /// <param name="propertyExpression">The expression returning the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder{TNextBuilder}"/>.</returns>
    public PropertyAttributesBuilder<ComponentAttributesBuilder<TComponent>> Property(Expression<Func<TComponent, object>> propertyExpression)
    {
        MemberInfo member = propertyExpression.CheckNotNull(nameof(propertyExpression)).ExtractMember();
        PropertyInfo property = (member as PropertyInfo)
            ?? throw new ArgumentException("Expression does not return a property.", nameof(propertyExpression));

        return new PropertyAttributesBuilder<ComponentAttributesBuilder<TComponent>>(
            _componentType,
            property.Name,
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

    protected override ComponentAttributesBuilder<TComponent> ResolveNextBuilder() => this;
}
