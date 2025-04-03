#nullable enable

namespace Atata;

/// <summary>
/// A builder of <typeparamref name="TComponent"/> component attributes.
/// </summary>
/// <typeparam name="TComponent">The type of the component.</typeparam>
public sealed class ComponentAttributesBuilder<TComponent> : AttributesBuilderBase
{
    private readonly AtataAttributesContext _attributesContext;

    private readonly Type _componentType;

    internal ComponentAttributesBuilder(
        AtataContextBuilder atataContextBuilder,
        AtataAttributesContext attributesContext)
        : base(atataContextBuilder)
    {
        _attributesContext = attributesContext;
        _componentType = typeof(TComponent);
    }

    /// <summary>
    /// Creates and returns the attributes builder for the property with the specified name.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder"/>.</returns>
    public PropertyAttributesBuilder this[string propertyName] =>
        Property(propertyName);

    /// <summary>
    /// Creates and returns the attributes builder for the property specified by expression.
    /// </summary>
    /// <param name="propertyExpression">The expression returning the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder"/>.</returns>
    public PropertyAttributesBuilder this[Expression<Func<TComponent, object>> propertyExpression] =>
        Property(propertyExpression);

    /// <summary>
    /// Configures this builder by action delegate.
    /// </summary>
    /// <param name="configure">An action delegate to configure the <see cref="ComponentAttributesBuilder{TComponent}"/>.</param>
    /// <returns>The <see cref="AtataContextBuilder"/> instance.</returns>
    public AtataContextBuilder Configure(Action<ComponentAttributesBuilder<TComponent>> configure)
    {
        configure.CheckNotNull(nameof(configure));

        configure(this);

        return AtataContextBuilder;
    }

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

    /// <summary>
    /// Creates and returns the attributes builder for the property specified by expression.
    /// </summary>
    /// <param name="propertyExpression">The expression returning the property.</param>
    /// <returns>An instance of <see cref="PropertyAttributesBuilder"/>.</returns>
    public PropertyAttributesBuilder Property(Expression<Func<TComponent, object>> propertyExpression)
    {
        MemberInfo member = propertyExpression.CheckNotNull(nameof(propertyExpression)).ExtractMember();
        PropertyInfo property = (member as PropertyInfo)
            ?? throw new ArgumentException("Expression does not return a property.", nameof(propertyExpression));

        return new(
            AtataContextBuilder,
            _attributesContext,
            _componentType,
            property.Name);
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
