using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Atata
{
    /// <summary>
    /// Represents the builder of <typeparamref name="TComponent"/> component attributes.
    /// </summary>
    /// <typeparam name="TComponent">The type of the component.</typeparam>
    public class ComponentAttributesAtataContextBuilder<TComponent>
        : AttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder<TComponent>>
    {
        private readonly Type _componentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentAttributesAtataContextBuilder{TComponent}"/> class.
        /// </summary>
        /// <param name="buildingContext">The building context.</param>
        public ComponentAttributesAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            _componentType = typeof(TComponent);
        }

        /// <summary>
        /// Creates and returns the attributes builder for the property with the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>An instance of <see cref="PropertyAttributesAtataContextBuilder{TNextBuilder}"/>.</returns>
        public PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder<TComponent>> this[string propertyName] =>
            Property(propertyName);

        /// <summary>
        /// Creates and returns the attributes builder for the property specified by expression.
        /// </summary>
        /// <param name="propertyExpression">The expression returning the property.</param>
        /// <returns>An instance of <see cref="PropertyAttributesAtataContextBuilder{TNextBuilder}"/>.</returns>
        public PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder<TComponent>> this[Expression<Func<TComponent, object>> propertyExpression] =>
            Property(propertyExpression);

        /// <summary>
        /// Creates and returns the attributes builder for the property with the specified name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>An instance of <see cref="PropertyAttributesAtataContextBuilder{TNextBuilder}"/>.</returns>
        public PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder<TComponent>> Property(string propertyName)
        {
            propertyName.CheckNotNullOrWhitespace(nameof(propertyName));

            return new PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder<TComponent>>(
                _componentType,
                propertyName,
                this,
                BuildingContext);
        }

        /// <summary>
        /// Creates and returns the attributes builder for the property specified by expression.
        /// </summary>
        /// <param name="propertyExpression">The expression returning the property.</param>
        /// <returns>An instance of <see cref="PropertyAttributesAtataContextBuilder{TNextBuilder}"/>.</returns>
        public PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder<TComponent>> Property(Expression<Func<TComponent, object>> propertyExpression)
        {
            MemberInfo member = propertyExpression.CheckNotNull(nameof(propertyExpression)).ExtractMember();
            PropertyInfo property = (member as PropertyInfo)
                ?? throw new ArgumentException("Expression does not return a property.", nameof(propertyExpression));

            return new PropertyAttributesAtataContextBuilder<ComponentAttributesAtataContextBuilder<TComponent>>(
                _componentType,
                property.Name,
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

        protected override ComponentAttributesAtataContextBuilder<TComponent> ResolveNextBuilder() => this;
    }
}
