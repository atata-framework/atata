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
            get { return TargetAttributeTypes?.FirstOrDefault(); }
            set { TargetAttributeTypes = value == null ? null : new[] { value }; }
        }

        public virtual int? CalculateTargetAttributeRank(Type targetAttributeType)
        {
            int? depthOfTypeInheritance = GetDepthOfInheritance(TargetAttributeTypes, targetAttributeType);
            if (depthOfTypeInheritance == null)
                return null;

            int rankFactor = 100000;

            return depthOfTypeInheritance >= 0
                ? Math.Max(rankFactor - (depthOfTypeInheritance.Value * 100), 0)
                : 0;
        }
    }
}
