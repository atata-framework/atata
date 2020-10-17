using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Represents the builder of property attributes.
    /// </summary>
    /// <typeparam name="TNextBuilder">The type of the next builder to return by <c>Add</c> methods.</typeparam>
    public class PropertyAttributesAtataContextBuilder<TNextBuilder>
        : AttributesAtataContextBuilder<TNextBuilder>
        where TNextBuilder : AttributesAtataContextBuilder
    {
        private readonly TypePropertyNamePair typeProperty;

        private readonly TNextBuilder parentBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyAttributesAtataContextBuilder{TNextBuilder}"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="parentBuilder">The parent builder.</param>
        /// <param name="buildingContext">The building context.</param>
        public PropertyAttributesAtataContextBuilder(Type type, string propertyName, TNextBuilder parentBuilder, AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            typeProperty = new TypePropertyNamePair(type, propertyName);
            this.parentBuilder = parentBuilder;
        }

        protected override void OnAdd(IEnumerable<Attribute> attributes)
        {
            if (!BuildingContext.Attributes.PropertyMap.TryGetValue(typeProperty, out var attributeSet))
            {
                attributeSet = new List<Attribute>();
                BuildingContext.Attributes.PropertyMap[typeProperty] = attributeSet;
            }

            attributeSet.AddRange(attributes);
        }

        protected override TNextBuilder ResolveNextBuilder() => parentBuilder;
    }
}
