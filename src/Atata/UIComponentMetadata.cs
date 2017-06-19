using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Atata
{
    public class UIComponentMetadata
    {
        public UIComponentMetadata(
            string name,
            Type componentType,
            Type parentComponentType)
        {
            Name = name;
            ComponentType = componentType;
            ParentComponentType = parentComponentType;
        }

        public string Name { get; private set; }

        public Type ComponentType { get; private set; }

        public Type ParentComponentType { get; private set; }

        public UIComponentDefinitionAttribute ComponentDefinitonAttribute { get; internal set; }

        internal List<Attribute> DeclaredAttributesList { get; set; }

        internal List<Attribute> ParentComponentAttributesList { get; set; }

        internal List<Attribute> AssemblyAttributesList { get; set; }

        internal List<Attribute> GlobalAttributesList { get; set; }

        internal List<Attribute> ComponentAttributesList { get; set; }

        [Obsolete("Use DeclaredAttributes instead.")]
        public IEnumerable<Attribute> DeclaringAttributes => DeclaredAttributes;

        public IEnumerable<Attribute> DeclaredAttributes => DeclaredAttributesList.AsEnumerable();

        public IEnumerable<Attribute> ComponentAttributes => ComponentAttributesList.AsEnumerable();

        public IEnumerable<Attribute> ParentComponentAttributes => ParentComponentAttributesList.AsEnumerable();

        public IEnumerable<Attribute> AssemblyAttributes => AssemblyAttributesList.AsEnumerable();

        public IEnumerable<Attribute> GlobalAttributes => GlobalAttributesList.AsEnumerable();

        public IEnumerable<Attribute> AllAttributes => DeclaredAttributesList.
            Concat(ParentComponentAttributesList).
            Concat(AssemblyAttributesList).
            Concat(GlobalAttributesList).
            Concat(ComponentAttributesList);

        /// <summary>
        /// Gets the first attribute of the specified type or null if no such attribute is found.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="levels">The attribute levels.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="filterByTarget">If set to <c>true</c>, filters by <see cref="MulticastAttribute"/> criteria if <typeparamref name="TAttribute"/> is <see cref="MulticastAttribute"/>.</param>
        /// <returns>The first attribute found or null.</returns>
        public TAttribute Get<TAttribute>(AttributeLevels levels, Func<TAttribute, bool> predicate = null, bool filterByTarget = true)
        {
            return GetAll(levels, predicate, filterByTarget).FirstOrDefault();
        }

        /// <summary>
        /// Gets the sequence of attributes of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <param name="levels">The attribute levels.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="filterByTarget">If set to <c>true</c>, filters by <see cref="MulticastAttribute"/> criteria if <typeparamref name="TAttribute"/> is <see cref="MulticastAttribute"/>.</param>
        /// <returns>The sequence of attributes found.</returns>
        public IEnumerable<TAttribute> GetAll<TAttribute>(AttributeLevels levels, Func<TAttribute, bool> predicate = null, bool filterByTarget = true)
        {
            var attributeSets = GetAllAttributeSets(levels);
            return FilterAttributeSets(attributeSets, predicate, filterByTarget);
        }

        private IEnumerable<IEnumerable<Attribute>> GetAllAttributeSets(AttributeLevels level)
        {
            IEnumerable<Attribute> result = Enumerable.Empty<Attribute>();

            if (level.HasFlag(AttributeLevels.Declared))
                yield return DeclaredAttributesList;

            if (level.HasFlag(AttributeLevels.ParentComponent))
                yield return ParentComponentAttributes;

            if (level.HasFlag(AttributeLevels.Assembly))
                yield return AssemblyAttributesList;

            if (level.HasFlag(AttributeLevels.Global))
                yield return GlobalAttributesList;

            if (level.HasFlag(AttributeLevels.Component))
                yield return ComponentAttributesList;
        }

        private IEnumerable<TAttribute> FilterAttributeSets<TAttribute>(IEnumerable<IEnumerable<Attribute>> attributeSets, Func<TAttribute, bool> predicate, bool filterByTarget)
        {
            bool shouldFilterByTarget = filterByTarget && typeof(MulticastAttribute).IsAssignableFrom(typeof(TAttribute));

            foreach (IEnumerable<Attribute> set in attributeSets)
            {
                var query = set.OfType<TAttribute>();

                if (predicate != null)
                    query = query.Where(predicate);

                if (shouldFilterByTarget)
                    query = FilterAndOrderByTarget(query);

                foreach (TAttribute attribute in query)
                    yield return attribute;
            }
        }

        private IEnumerable<TAttribute> FilterAndOrderByTarget<TAttribute>(IEnumerable<TAttribute> attributes)
        {
            return attributes.OfType<MulticastAttribute>().
                Select(x => new { Attribute = x, Rank = x.CalculateTargetRank(this) }).
                ToArray().
                Where(x => x.Rank.HasValue).
                OrderByDescending(x => x.Rank.Value).
                Select(x => x.Attribute).
                Cast<TAttribute>();
        }

        [Obsolete("Use Get(AttributeLevels.Declared, predicate) instead.")]
        public T GetFirstOrDefaultDeclaringAttribute<T>(Func<T, bool> predicate = null)
        {
            return Get(AttributeLevels.Declared, predicate);
        }

        [Obsolete("Use Get(AttributeLevels.ParentComponent | AttributeLevels.Assembly | AttributeLevels.Global, predicate) instead.")]
        public T GetFirstOrDefaultGlobalAttribute<T>(Func<T, bool> predicate = null)
        {
            return Get(AttributeLevels.ParentComponent | AttributeLevels.Assembly | AttributeLevels.Global, predicate);
        }

        [Obsolete("Use Get(AttributeLevels.Assembly, predicate) instead.")]
        public T GetFirstOrDefaultAssemblyAttribute<T>(Func<T, bool> predicate = null)
        {
            return Get(AttributeLevels.Assembly, predicate);
        }

        [Obsolete("Use Get(AttributeLevels.Component, predicate) instead.")]
        public T GetFirstOrDefaultComponentAttribute<T>(Func<T, bool> predicate = null)
        {
            return Get(AttributeLevels.Component, predicate);
        }

        [Obsolete("Use Get(AttributeLevels.All, predicate) instead.")]
        public T GetFirstOrDefaultAttribute<T>(Func<T, bool> predicate = null)
        {
            return Get(AttributeLevels.All, predicate);
        }

        [Obsolete("Use Get(AttributeLevels.DeclaredAndComponent, predicate) instead.")]
        public T GetFirstOrDefaultDeclaringOrComponentAttribute<T>(Func<T, bool> predicate = null)
        {
            return Get(AttributeLevels.DeclaredAndComponent, predicate);
        }

        [Obsolete("Use GetAll(AttributeLevels.Declared, predicate) instead.")]
        public IEnumerable<T> GetDeclaringAttributes<T>(Func<T, bool> predicate = null)
        {
            return GetAll(AttributeLevels.Declared, predicate);
        }

        [Obsolete("Use GetAll(AttributeLevels.NonComponent, predicate) instead.")]
        public IEnumerable<T> GetDeclaringAndGlobalAttributes<T>(Func<T, bool> predicate = null)
        {
            return GetAll(AttributeLevels.NonComponent, predicate);
        }

        [Obsolete("Use GetAll(AttributeLevels.ParentComponent | AttributeLevels.Assembly | AttributeLevels.Global, predicate) instead.")]
        public IEnumerable<T> GetGlobalAttributes<T>(Func<T, bool> predicate = null)
        {
            return GetAll(AttributeLevels.ParentComponent | AttributeLevels.Assembly | AttributeLevels.Global, predicate);
        }

        [Obsolete("Use GetAll(AttributeLevels.Assembly, predicate) instead.")]
        public IEnumerable<T> GetAssemblyAttributes<T>(Func<T, bool> predicate = null)
        {
            return GetAll(AttributeLevels.Assembly, predicate);
        }

        [Obsolete("Use GetAll(AttributeLevels.Component, predicate) instead.")]
        public IEnumerable<T> GetComponentAttributes<T>(Func<T, bool> predicate = null)
        {
            return GetAll(AttributeLevels.Component, predicate);
        }

        [Obsolete("Use Get<TermAttribute>(AttributeLevels.Declared, predicate) instead.")]
        public TermAttribute GetTerm(Func<TermAttribute, bool> predicate = null)
        {
            return Get(AttributeLevels.Declared, predicate);
        }

        /// <summary>
        /// Gets the culture by searching the <see cref="CultureAttribute"/> at any attribute level or current culture if not found.
        /// </summary>
        /// <returns>The <see cref="CultureInfo"/> instance.</returns>
        public CultureInfo GetCulture()
        {
            return (Get<CultureAttribute>(AttributeLevels.All, x => x.HasValue) ?? new CultureAttribute()).GetCultureInfo();
        }

        /// <summary>
        /// Gets the format by searching the <see cref="FormatAttribute"/> and <see cref="FormatSettingsAttribute"/> at any attribute level or null if not found.
        /// </summary>
        /// <returns>The format or null if not found.</returns>
        public string GetFormat()
        {
            return Get<FormatAttribute>(AttributeLevels.Declared)?.Value
                ?? Get<FormatSettingsAttribute>(
                    AttributeLevels.ParentComponent | AttributeLevels.Assembly | AttributeLevels.Global,
                    x => ComponentType.IsSubclassOfRawGeneric(x.ControlType))?.Value
                ?? Get<FormatAttribute>(AttributeLevels.Component)?.Value;
        }
    }
}
