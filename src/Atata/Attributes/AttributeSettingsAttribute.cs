using System;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute settings class for other attributes.
    /// </summary>
    public abstract class AttributeSettingsAttribute : MulticastAttribute
    {
        /// <summary>
        /// Gets or sets the target attribute types.
        /// </summary>
        public Type[] TargetAttributeTypes { get; set; }

        /// <summary>
        /// Gets or sets the target attribute type.
        /// </summary>
        public Type TargetAttributeType
        {
            get { return TargetAttributeTypes?.SingleOrDefault(); }
            set { TargetAttributeTypes = new[] { value }; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has any target specified.
        /// </summary>
        public override bool IsTargetSpecified =>
            base.IsTargetSpecified ||
            (TargetAttributeTypes?.Any() ?? false);
    }
}
