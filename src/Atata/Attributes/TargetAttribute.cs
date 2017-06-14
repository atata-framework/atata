using System;
using System.Linq;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class TargetAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the target component names.
        /// </summary>
        public string[] TargetNames { get; set; }

        /// <summary>
        /// Gets or sets the target component name.
        /// </summary>
        public string TargetName
        {
            get { return TargetNames.Single(); }
            set { TargetNames = new[] { value }; }
        }

        /// <summary>
        /// Gets or sets the target component types.
        /// </summary>
        public Type[] TargetTypes { get; set; }

        /// <summary>
        /// Gets or sets the target component type.
        /// </summary>
        public Type TargetType
        {
            get { return TargetTypes.Single(); }
            set { TargetTypes = new[] { value }; }
        }

        /// <summary>
        /// Gets or sets the target component's parent types.
        /// </summary>
        public Type[] TargetParentTypes { get; set; }

        /// <summary>
        /// Gets or sets the target component's parent type.
        /// </summary>
        public Type TargetParentType
        {
            get { return TargetParentTypes.Single(); }
            set { TargetParentTypes = new[] { value }; }
        }

        /// <summary>
        /// Determines whether the component name applies the name criteria.
        /// </summary>
        /// <param name="name">The component name.</param>
        /// <returns>
        ///   <c>true</c> if the name applies the criteria; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNameApplies(string name)
        {
            return TargetNames == null || !TargetNames.Any() || TargetNames.Contains(name);
        }

        /// <summary>
        /// Calculates the target rank.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The rank.</returns>
        public virtual int? CalculateTargetRank(UIComponentMetadata metadata)
        {
            if (!IsNameApplies(metadata.Name))
                return null;

            int? depthOfTypeInheritance = GetDepthOfInheritance(TargetTypes, metadata.ComponentType);
            if (depthOfTypeInheritance == null)
                return null;

            int? depthOfParentTypeInheritance = GetDepthOfInheritance(TargetParentTypes, metadata.ParentComponentType);
            if (depthOfParentTypeInheritance == null)
                return null;

            int rank = 0;
            int rankFactor = 100000;

            if (TargetNames != null && TargetNames.Any())
                rank += rankFactor;
            rankFactor /= 2;

            if (depthOfTypeInheritance >= 0)
                rank += Math.Max(rankFactor - (depthOfTypeInheritance.Value * 100), 0);
            rankFactor /= 2;

            if (depthOfParentTypeInheritance >= 0)
                rank += Math.Max(rankFactor - (depthOfParentTypeInheritance.Value * 100), 0);

            return rank;
        }

        private static int? GetDepthOfInheritance(Type[] targetTypes, Type typeToCheck)
        {
            if (targetTypes == null || !targetTypes.Any())
                return -1;
            else
                return targetTypes.Select(x => typeToCheck.GetDepthOfInheritance(x)).Where(x => x != null).Min();
        }
    }
}
