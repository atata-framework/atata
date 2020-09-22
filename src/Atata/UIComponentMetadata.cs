using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the UI component metadata which consists of component name, type, attributes, etc.
    /// </summary>
    public class UIComponentMetadata
    {
        private static readonly ControlDefinitionAttribute DefaultControlDefinitionAttribute =
            new ControlDefinitionAttribute { ComponentTypeName = "control" };

        private readonly AttributeSearchSet declaredAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.NonTargeted);

        private readonly AttributeSearchSet parentDeclaredAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.Targeted);

        private readonly AttributeSearchSet parentComponentAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.Targeted);

        private readonly AttributeSearchSet assemblyAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.All);

        private readonly AttributeSearchSet globalAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.All);

        private readonly AttributeSearchSet componentAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.NonTargeted);

        internal UIComponentMetadata(
            string name,
            Type componentType,
            Type parentComponentType)
        {
            Name = name;
            ComponentType = componentType;
            ParentComponentType = parentComponentType;
        }

        [Flags]
        private enum AttributeTargetFilterOptions
        {
            None = 0,
            Targeted = 1 << 0,
            NonTargeted = 1 << 1,
            All = Targeted | NonTargeted
        }

        /// <summary>
        /// Gets the name of the component.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the type of the component.
        /// </summary>
        public Type ComponentType { get; }

        /// <summary>
        /// Gets the type of the parent component.
        /// </summary>
        public Type ParentComponentType { get; }

        /// <summary>
        /// Gets the component definition attribute.
        /// </summary>
        public UIComponentDefinitionAttribute ComponentDefinitionAttribute =>
            ParentComponentType == null
                ? Get<PageObjectDefinitionAttribute>()
                : (Get<ControlDefinitionAttribute>() as UIComponentDefinitionAttribute ?? DefaultControlDefinitionAttribute);

        internal List<Attribute> DeclaredAttributesList => declaredAttributeSet.Attributes;

        internal List<Attribute> ParentDeclaredAttributesList
        {
            get => parentDeclaredAttributeSet.Attributes;
            set => parentDeclaredAttributeSet.Attributes = value;
        }

        internal List<Attribute> ParentComponentAttributesList => parentComponentAttributeSet.Attributes;

        internal List<Attribute> AssemblyAttributesList => assemblyAttributeSet.Attributes;

        internal List<Attribute> GlobalAttributesList
        {
            get => globalAttributeSet.Attributes;
            set => globalAttributeSet.Attributes = value;
        }

        internal List<Attribute> ComponentAttributesList => componentAttributeSet.Attributes;

        /// <summary>
        /// Gets the attributes hosted at the declared level.
        /// </summary>
        public IEnumerable<Attribute> DeclaredAttributes => DeclaredAttributesList.AsEnumerable();

        /// <summary>
        /// Gets the attributes hosted at the parent component level.
        /// </summary>
        public IEnumerable<Attribute> ParentComponentAttributes => ParentDeclaredAttributesList.Concat(ParentComponentAttributesList);

        /// <summary>
        /// Gets the attributes hosted at the assembly level.
        /// </summary>
        public IEnumerable<Attribute> AssemblyAttributes => AssemblyAttributesList.AsEnumerable();

        /// <summary>
        /// Gets the attributes hosted at the global level.
        /// </summary>
        public IEnumerable<Attribute> GlobalAttributes => GlobalAttributesList.AsEnumerable();

        /// <summary>
        /// Gets the attributes hosted at the component level.
        /// </summary>
        public IEnumerable<Attribute> ComponentAttributes => ComponentAttributesList.AsEnumerable();

        /// <summary>
        /// Gets all attributes in the following order of levels:
        /// <list type="number">
        /// <item>Declared</item>
        /// <item>Parent component</item>
        /// <item>Assembly</item>
        /// <item>Global</item>
        /// <item>Component</item>
        /// </list>
        /// </summary>
        public IEnumerable<Attribute> AllAttributes => DeclaredAttributesList.
            Concat(ParentDeclaredAttributesList).
            Concat(ParentComponentAttributesList).
            Concat(AssemblyAttributesList).
            Concat(GlobalAttributesList).
            Concat(ComponentAttributesList);

        /// <summary>
        /// Gets the first attribute of the specified type or <see langword="null"/> if no such attribute is found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>The first attribute found or <see langword="null"/>.</returns>
        public TAttribute Get<TAttribute>()
        {
            return Get<TAttribute>(null);
        }

        /// <summary>
        /// Gets the first attribute of the specified type or <see langword="null"/> if no such attribute is found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="filterConfiguration">The filter configuration function.</param>
        /// <returns>The first attribute found or <see langword="null"/>.</returns>
        public TAttribute Get<TAttribute>(Func<AttributeFilter<TAttribute>, AttributeFilter<TAttribute>> filterConfiguration)
        {
            return GetAll(filterConfiguration).FirstOrDefault();
        }

        /// <summary>
        /// Gets the first attribute of the specified type or <see langword="null"/> if no such attribute is found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="levels">The attribute levels.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="filterByTarget">
        /// If set to <see langword="true"/>, filters by <see cref="MulticastAttribute"/> criteria
        /// in case <typeparamref name="TAttribute"/> is <see cref="MulticastAttribute"/>.
        /// </param>
        /// <returns>The first attribute found or <see langword="null"/>.</returns>
        [Obsolete("Use Get() or Get(filterConfiguration) instead.")] // Obsolete since v1.0.0.
        public TAttribute Get<TAttribute>(AttributeLevels levels, Func<TAttribute, bool> predicate = null, bool filterByTarget = true)
        {
            return GetAll(levels, predicate, filterByTarget).FirstOrDefault();
        }

        /// <summary>
        /// Gets the sequence of attributes of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>The sequence of attributes found.</returns>
        public IEnumerable<TAttribute> GetAll<TAttribute>()
        {
            return GetAll<TAttribute>(null);
        }

        /// <summary>
        /// Gets the sequence of attributes of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="filterConfiguration">The filter configuration function.</param>
        /// <returns>The sequence of attributes found.</returns>
        public IEnumerable<TAttribute> GetAll<TAttribute>(Func<AttributeFilter<TAttribute>, AttributeFilter<TAttribute>> filterConfiguration)
        {
            AttributeFilter<TAttribute> defaultFilter = new AttributeFilter<TAttribute>();

            AttributeFilter<TAttribute> filter = filterConfiguration?.Invoke(defaultFilter) ?? defaultFilter;

            var attributeSets = GetAllAttributeSets(filter.Levels);

            return FilterAttributeSets(attributeSets, filter, true);
        }

        /// <summary>
        /// Gets the sequence of attributes of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="levels">The attribute levels.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="filterByTarget">
        /// If set to <see langword="true"/>, filters by <see cref="MulticastAttribute"/> criteria
        /// in case <typeparamref name="TAttribute"/> is <see cref="MulticastAttribute"/>.
        /// </param>
        /// <returns>The sequence of attributes found.</returns>
        [Obsolete("Use GetAll() or GetAll(filterConfiguration) instead.")] // Obsolete since v1.0.0.
        public IEnumerable<TAttribute> GetAll<TAttribute>(AttributeLevels levels, Func<TAttribute, bool> predicate = null, bool filterByTarget = true)
        {
            AttributeFilter<TAttribute> filter = new AttributeFilter<TAttribute>().At(levels).Where(predicate);

            var attributeSets = GetAllAttributeSets(filter.Levels);
            return FilterAttributeSets(attributeSets, filter, filterByTarget);
        }

        private IEnumerable<AttributeSearchSet> GetAllAttributeSets(AttributeLevels level)
        {
            if (level.HasFlag(AttributeLevels.Declared))
                yield return declaredAttributeSet;

            if (level.HasFlag(AttributeLevels.ParentComponent))
            {
                yield return parentDeclaredAttributeSet;
                yield return parentComponentAttributeSet;
            }

            if (level.HasFlag(AttributeLevels.Assembly))
                yield return assemblyAttributeSet;

            if (level.HasFlag(AttributeLevels.Global))
                yield return globalAttributeSet;

            if (level.HasFlag(AttributeLevels.Component))
                yield return componentAttributeSet;
        }

        private IEnumerable<AttributeSearchSet> GetAllAttributeSetsInLayersOrdered()
        {
            yield return globalAttributeSet;
            yield return assemblyAttributeSet;
            yield return parentDeclaredAttributeSet;
            yield return parentComponentAttributeSet;
            yield return declaredAttributeSet;
            yield return componentAttributeSet;
        }

        // TODO: filterByTarget should be removed.
        private IEnumerable<TAttribute> FilterAttributeSets<TAttribute>(
            IEnumerable<AttributeSearchSet> attributeSets,
            AttributeFilter<TAttribute> filter,
            bool filterByTarget)
        {
            bool shouldFilterByTarget = filterByTarget && typeof(MulticastAttribute).IsAssignableFrom(typeof(TAttribute));

            foreach (AttributeSearchSet set in attributeSets)
            {
                var query = set.Attributes.OfType<TAttribute>();

                if (shouldFilterByTarget)
                    query = FilterAndOrderByTarget(query, filter, set.TargetFilterOptions);

                foreach (var predicate in filter.Predicates)
                    query = query.Where(predicate);

                foreach (TAttribute attribute in query)
                    yield return attribute;
            }
        }

        private IEnumerable<TAttribute> FilterAndOrderByTarget<TAttribute>(IEnumerable<TAttribute> attributes, AttributeFilter<TAttribute> filter, AttributeTargetFilterOptions targetFilterOptions)
        {
            if (targetFilterOptions == AttributeTargetFilterOptions.None)
                return Enumerable.Empty<TAttribute>();

            var query = attributes.OfType<MulticastAttribute>();

            if (targetFilterOptions == AttributeTargetFilterOptions.Targeted)
                query = query.Where(x => x.IsTargetSpecified);
            else if (targetFilterOptions == AttributeTargetFilterOptions.NonTargeted)
                query = query.Where(x => !x.IsTargetSpecified);

            var rankedQuery = query.
                Select(x => new { Attribute = x, TargetRank = x.CalculateTargetRank(this) }).
                Where(x => x.TargetRank.HasValue);

            if (filter.TargetAttributeType != null && typeof(AttributeSettingsAttribute).IsAssignableFrom(typeof(TAttribute)))
            {
                return rankedQuery.
                    Select(x => new { x.Attribute, x.TargetRank, TargetAttributeRank = ((AttributeSettingsAttribute)x.Attribute).CalculateTargetAttributeRank(filter.TargetAttributeType) }).
                    Where(x => x.TargetAttributeRank.HasValue).
                    OrderByDescending(x => x.TargetRank.Value).
                    ThenByDescending(x => x.TargetAttributeRank.Value).
                    Select(x => x.Attribute).
                    Cast<TAttribute>();
            }
            else
            {
                return rankedQuery.
                    OrderByDescending(x => x.TargetRank.Value).
                    Select(x => x.Attribute).
                    Cast<TAttribute>();
            }
        }

        /// <summary>
        /// Inserts the specified attributes into <see cref="DeclaredAttributes"/> collection at the beginning.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public void Push(params Attribute[] attributes)
        {
            Push(attributes as IEnumerable<Attribute>);
        }

        /// <summary>
        /// Inserts the specified attributes into <see cref="DeclaredAttributes"/> collection at the beginning.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public void Push(IEnumerable<Attribute> attributes)
        {
            if (attributes != null)
                DeclaredAttributesList.InsertRange(0, attributes);
        }

        /// <summary>
        /// Gets the culture by searching the <see cref="CultureAttribute"/> at all attribute levels or current culture if not found.
        /// </summary>
        /// <returns>The <see cref="CultureInfo"/> instance.</returns>
        public CultureInfo GetCulture()
        {
            string cultureName = Get<CultureAttribute>()?.Value;

            return cultureName != null ? CultureInfo.GetCultureInfo(cultureName) : CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// Gets the format by searching the <see cref="FormatAttribute"/> at all attribute levels or <see langword="null"/> if not found.
        /// </summary>
        /// <returns>The format or <see langword="null"/> if not found.</returns>
        public string GetFormat()
        {
            return Get<FormatAttribute>()?.Value;
        }

        /// <summary>
        /// Gets the format to use for getting the value of the control.
        /// Searches for the <see cref="ValueGetFormatAttribute"/> or <see cref="FormatAttribute"/> at all attribute levels.
        /// Returns <see langword="null"/> when neither attribute found.
        /// </summary>
        /// <returns>The format or <see langword="null"/> if not found.</returns>
        // TODO: Update GetValueGetFormat method to consider all formatting attributes, like behaviors. Then make it public and use it.
        internal string GetValueGetFormat()
        {
            ValueGetFormatAttribute valueGetFormatAttribute = Get<ValueGetFormatAttribute>();

            return valueGetFormatAttribute != null
                ? valueGetFormatAttribute.Value
                : Get<FormatAttribute>()?.Value;
        }

        /// <summary>
        /// Gets the format to use for setting the value to the control.
        /// Searches for the <see cref="ValueSetFormatAttribute"/> or <see cref="FormatAttribute"/> at all attribute levels.
        /// Returns <see langword="null"/> when neither attribute found.
        /// </summary>
        /// <returns>The format or <see langword="null"/> if not found.</returns>
        // TODO: Update GetValueSetFormat method to consider all formatting attributes, like behaviors. Then make it public and use it.
        internal string GetValueSetFormat()
        {
            ValueSetFormatAttribute valueSetFormatAttribute = Get<ValueSetFormatAttribute>();

            return valueSetFormatAttribute != null
                ? valueSetFormatAttribute.Value
                : Get<FormatAttribute>()?.Value;
        }

        public FindAttribute ResolveFindAttribute()
        {
            // TODO: Use the below aproach in v2.0.0.
            ////FindAttribute findAttribute = GetDefinedFindAttribute();

            FindAttribute findAttribute = Get<FindAttribute>(filter => filter.At(AttributeLevels.Declared).Where(x => x.As == FindAs.Scope))
#pragma warning disable CS0618 // Type or member is obsolete
                ?? ResolveNonDefinedFindAttribute()
#pragma warning restore CS0618 // Type or member is obsolete
                ?? GetDefinedFindAttribute()
                ?? GetDefaultFindAttribute();

            findAttribute.Properties.Metadata = this;

            return findAttribute;
        }

        private FindAttribute GetDefinedFindAttribute()
        {
            return Get<FindAttribute>(filter => filter.Where(x => x.As == FindAs.Scope));
        }

        [Obsolete("Should be removed in v2.0.0")]
        private FindAttribute ResolveNonDefinedFindAttribute()
        {
            ControlFindingAttribute controlFindingAttribute =
                GetNearestControlFindingAttribute(ParentComponentAttributes) ??
                GetNearestControlFindingAttribute(AssemblyAttributes) ??
                GetNearestDefaultControlFindingAttribute();

            return controlFindingAttribute?.CreateFindAttribute();
        }

        [Obsolete("Should be removed in v2.0.0")]
        private ControlFindingAttribute GetNearestControlFindingAttribute(IEnumerable<Attribute> attributes)
        {
            Type controlType = ComponentType;
            Type parentComponentType = ParentComponentType;

            return attributes.OfType<ControlFindingAttribute>().
                Select(attr => new { Attribute = attr, Depth = controlType.GetDepthOfInheritance(attr.ControlType) }).
                Where(x => x.Depth != null).
                OrderBy(x => x.Depth).
                Select(x => x.Attribute).
                FirstOrDefault(attr => attr.ParentComponentType == null || parentComponentType.IsInheritedFromOrIs(attr.ParentComponentType));
        }

        [Obsolete("Should be removed in v2.0.0")]
        private ControlFindingAttribute GetNearestDefaultControlFindingAttribute()
        {
            Type parentComponentType = ParentComponentType;

            var allFindingAttributes = ComponentAttributes.OfType<ControlFindingAttribute>().
                Where(x => x.ControlType == null).
                Select(attr => new { Attribute = attr, Depth = parentComponentType.GetDepthOfInheritance(attr.ParentComponentType) }).
                ToArray();

            return allFindingAttributes.Where(x => x.Depth != null).OrderBy(x => x.Depth).Select(x => x.Attribute).FirstOrDefault() ??
                allFindingAttributes.Where(x => x.Depth == null && x.Attribute.ParentComponentType == null).Select(x => x.Attribute).FirstOrDefault();
        }

        private FindAttribute GetDefaultFindAttribute()
        {
            if (ComponentDefinitionAttribute.ScopeXPath == ScopeDefinitionAttribute.DefaultScopeXPath && !GetLayerFindAttributes().Any())
                return new UseParentScopeAttribute();

            return new FindFirstAttribute();
        }

        public IEnumerable<FindAttribute> ResolveLayerFindAttributes()
        {
            var attributes = GetLayerFindAttributes()
                .OrderBy(x => x.Layer)
                .ToArray();

            if (attributes.Any())
            {
                UIComponentMetadata metadata = CreateMetadataForLayerFindAttribute();

                foreach (FindAttribute attribute in attributes)
                    attribute.Properties.Metadata = metadata;
            }

            return attributes;
        }

        private IEnumerable<FindAttribute> GetLayerFindAttributes()
        {
            var attributeSets = GetAllAttributeSetsInLayersOrdered();

            var filter = new AttributeFilter<FindAttribute>()
                .Where(x => x.As != FindAs.Scope);

            return FilterAttributeSets(attributeSets, filter, true);
        }

        private UIComponentMetadata CreateMetadataForLayerFindAttribute()
        {
            bool LocalFilter(Attribute a) => a is TermAttribute;

            UIComponentMetadata metadata = new UIComponentMetadata(Name, ComponentType, ParentComponentType);

            metadata.DeclaredAttributesList.AddRange(DeclaredAttributesList.Where(LocalFilter));
            metadata.ParentDeclaredAttributesList.AddRange(ParentDeclaredAttributesList.Where(LocalFilter));
            metadata.ParentComponentAttributesList.AddRange(ParentComponentAttributesList.Where(LocalFilter));
            metadata.AssemblyAttributesList.AddRange(AssemblyAttributesList.Where(LocalFilter));
            metadata.GlobalAttributesList.AddRange(GlobalAttributesList.Where(LocalFilter));
            metadata.ComponentAttributesList.AddRange(ComponentAttributesList.Where(LocalFilter));

            return metadata;
        }

        private class AttributeSearchSet
        {
            public AttributeSearchSet(AttributeTargetFilterOptions targetFilterOptions) =>
                TargetFilterOptions = targetFilterOptions;

            public List<Attribute> Attributes { get; set; } = new List<Attribute>();

            public AttributeTargetFilterOptions TargetFilterOptions { get; }
        }
    }
}
