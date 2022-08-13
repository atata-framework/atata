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
        private static readonly ControlDefinitionAttribute s_defaultControlDefinitionAttribute =
            new ControlDefinitionAttribute { ComponentTypeName = "control" };

        private readonly AttributeSearchSet _declaredAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.NonTargeted);

        private readonly AttributeSearchSet _parentDeclaredAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.Targeted);

        private readonly AttributeSearchSet _parentComponentAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.Targeted);

        private readonly AttributeSearchSet _assemblyAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.All);

        private readonly AttributeSearchSet _globalAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.All);

        private readonly AttributeSearchSet _componentAttributeSet = new AttributeSearchSet(AttributeTargetFilterOptions.NonTargeted);

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
                : (Get<ControlDefinitionAttribute>() as UIComponentDefinitionAttribute ?? s_defaultControlDefinitionAttribute);

        internal List<Attribute> DeclaredAttributesList => _declaredAttributeSet.Attributes;

        internal List<Attribute> ParentDeclaredAttributesList
        {
            get => _parentDeclaredAttributeSet.Attributes;
            set => _parentDeclaredAttributeSet.Attributes = value;
        }

        internal List<Attribute> ParentComponentAttributesList
        {
            get => _parentComponentAttributeSet.Attributes;
            set => _parentComponentAttributeSet.Attributes = value;
        }

        internal List<Attribute> AssemblyAttributesList => _assemblyAttributeSet.Attributes;

        internal List<Attribute> GlobalAttributesList => _globalAttributeSet.Attributes;

        internal List<Attribute> ComponentAttributesList => _componentAttributeSet.Attributes;

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
        /// Determines whether this instance contains the attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns><see langword="true"/> if contains; otherwise, <see langword="false"/>.</returns>
        public bool Contains<TAttribute>() =>
            Get<TAttribute>() != null;

        /// <summary>
        /// Tries to get the first attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="attribute">The attribute.</param>
        /// <returns><see langword="true"/> if attribute is found; otherwise, <see langword="false"/>.</returns>
        public bool TryGet<TAttribute>(out TAttribute attribute)
        {
            attribute = Get<TAttribute>();
            return attribute != null;
        }

        /// <summary>
        /// Gets the first attribute of the specified type or <see langword="null"/> if no such attribute is found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>The first attribute found or <see langword="null"/>.</returns>
        public TAttribute Get<TAttribute>() =>
            Get<TAttribute>(null);

        /// <summary>
        /// Gets the first attribute of the specified type or <see langword="null"/> if no such attribute is found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="filterConfiguration">The filter configuration function.</param>
        /// <returns>The first attribute found or <see langword="null"/>.</returns>
        public TAttribute Get<TAttribute>(Func<AttributeFilter<TAttribute>, AttributeFilter<TAttribute>> filterConfiguration) =>
            GetAll(filterConfiguration).FirstOrDefault();

        /// <summary>
        /// Gets the sequence of attributes of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>The sequence of attributes found.</returns>
        public IEnumerable<TAttribute> GetAll<TAttribute>() =>
            GetAll<TAttribute>(null);

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

            return FilterAttributeSets(attributeSets, filter);
        }

        private IEnumerable<AttributeSearchSet> GetAllAttributeSets(AttributeLevels level)
        {
            if (level.HasFlag(AttributeLevels.Declared))
                yield return _declaredAttributeSet;

            if (level.HasFlag(AttributeLevels.ParentComponent))
            {
                yield return _parentDeclaredAttributeSet;
                yield return _parentComponentAttributeSet;
            }

            if (level.HasFlag(AttributeLevels.Assembly))
                yield return _assemblyAttributeSet;

            if (level.HasFlag(AttributeLevels.Global))
                yield return _globalAttributeSet;

            if (level.HasFlag(AttributeLevels.Component))
                yield return _componentAttributeSet;
        }

        private IEnumerable<AttributeSearchSet> GetAllAttributeSetsInLayersOrdered()
        {
            yield return _globalAttributeSet;
            yield return _assemblyAttributeSet;
            yield return _parentDeclaredAttributeSet;
            yield return _parentComponentAttributeSet;
            yield return _declaredAttributeSet;
            yield return _componentAttributeSet;
        }

        private IEnumerable<TAttribute> FilterAttributeSets<TAttribute>(
            IEnumerable<AttributeSearchSet> attributeSets,
            AttributeFilter<TAttribute> filter)
        {
            bool shouldFilterByTarget = typeof(MulticastAttribute).IsAssignableFrom(typeof(TAttribute));

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
                query = query.Where(x => x.TargetSelf);
            else
                query = query.Where(x => x.TargetSelf || x.IsTargetSpecified);

            var rankedQuery = query
                .Select(x => new { Attribute = x, TargetRank = x.CalculateTargetRank(this) })
                .Where(x => x.TargetRank.HasValue)
                .ToArray();

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
        public void Push(params Attribute[] attributes) =>
            Push(attributes.AsEnumerable());

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
        /// Adds the specified attributes to <see cref="DeclaredAttributes"/> collection at the end.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public void Add(params Attribute[] attributes) =>
            Add(attributes.AsEnumerable());

        /// <summary>
        /// Adds the specified attributes to <see cref="DeclaredAttributes"/> collection at the end.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        public void Add(IEnumerable<Attribute> attributes)
        {
            if (attributes != null)
                DeclaredAttributesList.AddRange(attributes);
        }

        /// <summary>
        /// Removes the specified attributes from <see cref="DeclaredAttributes" /> collection.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns><see langword="true"/> if at least one item is successfully removed; otherwise, <see langword="false"/>.</returns>
        public bool Remove(params Attribute[] attributes) =>
            Remove(attributes.AsEnumerable());

        /// <summary>
        /// Removes the specified attributes from <see cref="DeclaredAttributes"/> collection.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns><see langword="true"/> if at least one item is successfully removed; otherwise, <see langword="false"/>.</returns>
        public bool Remove(IEnumerable<Attribute> attributes)
        {
            attributes.CheckNotNull(nameof(attributes));

            bool isRemoved = false;

            foreach (Attribute attribute in attributes)
            {
                isRemoved |= DeclaredAttributesList.Remove(attribute);
            }

            return isRemoved;
        }

        /// <summary>
        /// Removes all the attributes that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns>The number of removed elements.</returns>
        public int RemoveAll(Predicate<Attribute> match)
        {
            match.CheckNotNull(nameof(match));

            return DeclaredAttributesList.RemoveAll(match);
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
        public string GetFormat() =>
            Get<FormatAttribute>()?.Value;

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

        public FindAttribute ResolveFindAttribute() =>
            GetDefinedFindAttribute()
                ?? GetDefaultFindAttribute();

        private FindAttribute GetDefinedFindAttribute() =>
            Get<FindAttribute>(filter => filter.Where(x => x.As == FindAs.Scope));

        private FindAttribute GetDefaultFindAttribute()
        {
            if (ComponentDefinitionAttribute.ScopeXPath == ScopeDefinitionAttribute.DefaultScopeXPath && !GetLayerFindAttributes().Any())
                return new UseParentScopeAttribute();
            else
                return new FindFirstAttribute();
        }

        public IEnumerable<FindAttribute> ResolveLayerFindAttributes() =>
            GetLayerFindAttributes()
                .OrderBy(x => x.Layer);

        private IEnumerable<FindAttribute> GetLayerFindAttributes()
        {
            var attributeSets = GetAllAttributeSetsInLayersOrdered();

            var filter = new AttributeFilter<FindAttribute>()
                .Where(x => x.As != FindAs.Scope);

            return FilterAttributeSets(attributeSets, filter);
        }

        internal UIComponentMetadata CreateMetadataForLayerFindAttribute()
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

        private sealed class AttributeSearchSet
        {
            public AttributeSearchSet(AttributeTargetFilterOptions targetFilterOptions) =>
                TargetFilterOptions = targetFilterOptions;

            public List<Attribute> Attributes { get; set; } = new List<Attribute>();

            public AttributeTargetFilterOptions TargetFilterOptions { get; }
        }
    }
}
