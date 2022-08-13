using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Represents the builder of global level attributes.
    /// </summary>
    public class GlobalAttributesAtataContextBuilder : AttributesAtataContextBuilder<GlobalAttributesAtataContextBuilder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalAttributesAtataContextBuilder"/> class.
        /// </summary>
        /// <param name="buildingContext">The building context.</param>
        public GlobalAttributesAtataContextBuilder(AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
        }

        protected override void OnAdd(IEnumerable<Attribute> attributes) =>
            BuildingContext.Attributes.Global.AddRange(attributes);

        protected override GlobalAttributesAtataContextBuilder ResolveNextBuilder() => this;
    }
}
