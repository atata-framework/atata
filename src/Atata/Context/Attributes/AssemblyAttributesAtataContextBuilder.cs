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
        private readonly Assembly _assembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyAttributesAtataContextBuilder"/> class.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="buildingContext">The building context.</param>
        public AssemblyAttributesAtataContextBuilder(Assembly assembly, AtataBuildingContext buildingContext)
            : base(buildingContext) =>
            _assembly = assembly;

        protected override void OnAdd(IEnumerable<Attribute> attributes)
        {
            if (!BuildingContext.Attributes.AssemblyMap.TryGetValue(_assembly, out var attributeSet))
            {
                attributeSet = new List<Attribute>();
                BuildingContext.Attributes.AssemblyMap[_assembly] = attributeSet;
            }

            attributeSet.AddRange(attributes);
        }

        protected override AssemblyAttributesAtataContextBuilder ResolveNextBuilder() => this;
    }
}
