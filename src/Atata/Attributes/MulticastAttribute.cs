using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base class for Atata attributes that can be applied to component at any level (declared, parent component, assembly, global and component).
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Assembly,
        AllowMultiple = true)]
    public abstract class MulticastAttribute : Attribute, IPropertySettings
    {
        private bool? _targetSelf;

        /// <summary>
        /// Gets or sets the target component names.
        /// </summary>
        public string[] TargetNames { get; set; }

        /// <summary>
        /// Gets or sets the target component name.
        /// </summary>
        public string TargetName
        {
            get => TargetNames?.FirstOrDefault();
            set => TargetNames = value == null ? null : new[] { value };
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
            get => TargetTypes?.FirstOrDefault();
            set => TargetTypes = value == null ? null : new[] { value };
        }

        /// <summary>
        /// Gets or sets the target component tags.
        /// </summary>
        public string[] TargetTags { get; set; }

        /// <summary>
        /// Gets or sets the target component tag.
        /// </summary>
        public string TargetTag
        {
            get => TargetTags?.FirstOrDefault();
            set => TargetTags = value == null ? null : new[] { value };
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
            get => TargetParentTypes?.FirstOrDefault();
            set => TargetParentTypes = value == null ? null : new[] { value };
        }

        /// <summary>
        /// Gets or sets the target component names to exlcude.
        /// </summary>
        public string[] ExcludeTargetNames { get; set; }

        /// <summary>
        /// Gets or sets the target component name to exclude.
        /// </summary>
        public string ExcludeTargetName
        {
            get => ExcludeTargetNames?.FirstOrDefault();
            set => ExcludeTargetNames = value == null ? null : new[] { value };
        }

        /// <summary>
        /// Gets or sets the target component types to exclude.
        /// </summary>
        public Type[] ExcludeTargetTypes { get; set; }

        /// <summary>
        /// Gets or sets the target component type to exclude.
        /// </summary>
        public Type ExcludeTargetType
        {
            get => ExcludeTargetTypes?.FirstOrDefault();
            set => ExcludeTargetTypes = value == null ? null : new[] { value };
        }

        /// <summary>
        /// Gets or sets the target component tags to exclude.
        /// </summary>
        public string[] ExcludeTargetTags { get; set; }

        /// <summary>
        /// Gets or sets the target component tag to exclude.
        /// </summary>
        public string ExcludeTargetTag
        {
            get => ExcludeTargetTags?.FirstOrDefault();
            set => ExcludeTargetTags = value == null ? null : new[] { value };
        }

        /// <summary>
        /// Gets or sets the target component's parent types to exclude.
        /// </summary>
        public Type[] ExcludeTargetParentTypes { get; set; }

        /// <summary>
        /// Gets or sets the target component's parent type to exclude.
        /// </summary>
        public Type ExcludeTargetParentType
        {
            get => ExcludeTargetParentTypes?.FirstOrDefault();
            set => ExcludeTargetParentTypes = value == null ? null : new[] { value };
        }

        /// <summary>
        /// Gets or sets a value indicating whether to target the component where this attribute is declared.
        /// </summary>
        public bool TargetSelf
        {
            get => _targetSelf ?? !IsTargetSpecified;
            set => _targetSelf = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to target the component where this attribute is declared and its children.
        /// </summary>
        public bool TargetSelfAndChildren
        {
            get => TargetSelf && TargetChildren;
            set
            {
                TargetSelf = value;
                TargetChildren = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has any target specified.
        /// </summary>
        public virtual bool IsTargetSpecified =>
            (TargetNames?.Any() ?? false) ||
            (TargetTypes?.Any() ?? false) ||
            (TargetTags?.Any() ?? false) ||
            (TargetParentTypes?.Any() ?? false) ||
            (ExcludeTargetNames?.Any() ?? false) ||
            (ExcludeTargetTypes?.Any() ?? false) ||
            (ExcludeTargetTags?.Any() ?? false) ||
            (ExcludeTargetParentTypes?.Any() ?? false);

        /// <summary>
        /// Gets or sets a value indicating whether any type is targeted.
        /// When is set to <see langword="true"/>, sets <c>typeof(object)</c> to <see cref="TargetType"/> property.
        /// When is set to <see langword="false"/>, sets <see langword="null"/> to <see cref="TargetType"/> property.
        /// </summary>
        public bool TargetAnyType
        {
            get => TargetTypes?.Any(x => x == typeof(object)) ?? false;
            set => TargetTypes = value ? new[] { typeof(object) } : null;
        }

        /// <summary>
        /// Gets or sets a value indicating whether all children are targeted.
        /// Actually, is a wrapper over the <see cref="TargetAnyType"/> property.
        /// </summary>
        public bool TargetAllChildren
        {
            get => TargetAnyType;
            set => TargetAnyType = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether children are targeted.
        /// </summary>
        public bool TargetChildren
        {
            get => IsTargetSpecified;
            set
            {
                if (value)
                {
                    if (!IsTargetSpecified)
                        TargetAnyType = value;
                }
                else
                {
                    TargetNames = null;
                    TargetTypes = null;
                    TargetTags = null;
                    TargetParentTypes = null;
                    ExcludeTargetNames = null;
                    ExcludeTargetTypes = null;
                    ExcludeTargetTags = null;
                    ExcludeTargetParentTypes = null;
                }
            }
        }

        /// <summary>
        /// Gets the properties bag.
        /// </summary>
        public PropertyBag Properties { get; } = new PropertyBag();

        /// <summary>
        /// Determines whether the component name applies the name criteria.
        /// </summary>
        /// <param name="name">The component name.</param>
        /// <returns>
        /// <see langword="true"/> if the name applies the criteria; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsNameApplicable(string name)
        {
            return (TargetNames == null || !TargetNames.Any() || TargetNames.Contains(name))
                && (ExcludeTargetNames == null || !ExcludeTargetNames.Any() || !ExcludeTargetNames.Contains(name));
        }

        /// <summary>
        /// Determines whether the component tags apply the tag criteria.
        /// </summary>
        /// <param name="tags">The component tags.</param>
        /// <returns>
        /// <see langword="true"/> if the tags apply the criteria; otherwise, <see langword="false"/>.
        /// </returns>
        public bool AreTagsApplicable(IEnumerable<string> tags)
        {
            tags.CheckNotNull(nameof(tags));

            return (TargetTags == null || !TargetTags.Any() || TargetTags.Intersect(tags).Any())
                && (ExcludeTargetTags == null || !ExcludeTargetTags.Any() || !ExcludeTargetTags.Intersect(tags).Any());
        }

        /// <summary>
        /// Calculates the target rank.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <returns>The rank.</returns>
        public virtual int? CalculateTargetRank(UIComponentMetadata metadata)
        {
            if (!IsNameApplicable(metadata.Name))
                return null;

            int? depthOfTypeInheritance = GetDepthOfInheritance(metadata.ComponentType, TargetTypes, ExcludeTargetTypes);
            if (depthOfTypeInheritance == null)
                return null;

            var tags = metadata.GetAll<TagAttribute>().SelectMany(x => x.Values).Distinct().ToArray();
            if (!AreTagsApplicable(tags))
                return null;

            int? depthOfParentTypeInheritance = GetDepthOfInheritance(metadata.ParentComponentType, TargetParentTypes, ExcludeTargetParentTypes);
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

            if (TargetTags != null && TargetTags.Any())
                rank += rankFactor;
            rankFactor /= 2;

            if (depthOfParentTypeInheritance >= 0)
                rank += Math.Max(rankFactor - (depthOfParentTypeInheritance.Value * 100), 0);

            return rank;
        }

        [Obsolete("Use GetDepthOfInheritance(Type, Type[], Type[]) instead.")] // Obsolete since v1.10.0.
        protected static int? GetDepthOfInheritance(Type[] targetTypes, Type typeToCheck) =>
            GetDepthOfInheritance(typeToCheck, targetTypes);

        protected static int? GetDepthOfInheritance(Type typeToCheck, Type[] targetTypes, Type[] excludeTargetTypes = null)
        {
            if (excludeTargetTypes?.Any(x => typeToCheck.IsInheritedFromOrIs(x)) ?? false)
                return null;

            return targetTypes == null || !targetTypes.Any()
                ? -1
                : targetTypes.Select(x => typeToCheck.GetDepthOfInheritance(x)).Where(x => x != null).Min();
        }
    }
}
