using System;
using System.Collections.Generic;
using System.Reflection;

namespace Atata
{
    /// <summary>
    /// Represents the builder of assembly attributes.
    /// </summary>
    public class AssemblyAttributesAtataContextBuilder
        : AttributesAtataContextBuilder<AssemblyAttributesAtataContextBuilder>
    {
        private readonly Assembly assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyAttributesAtataContextBuilder"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="buildingContext">The building context.</param>
        public AssemblyAttributesAtataContextBuilder(Assembly assembly, AtataBuildingContext buildingContext)
            : base(buildingContext)
        {
            this.assembly = assembly;
        }

        protected override void OnAdd(IEnumerable<Attribute> attributes)
        {
            if (!BuildingContext.Attributes.AssemblyMap.TryGetValue(assembly, out var attributeSet))
            {
                attributeSet = new List<Attribute>();
                BuildingContext.Attributes.AssemblyMap[assembly] = attributeSet;
            }

            attributeSet.AddRange(attributes);
        }

        protected override AssemblyAttributesAtataContextBuilder ResolveNextBuilder() => this;
    }
}
