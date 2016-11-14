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
            Type parentComponentType,
            Attribute[] declaringAttributes,
            Attribute[] componentAttributes,
            Attribute[] parentComponentAttributes,
            Attribute[] assemblyAttributes)
        {
            Name = name;
            ComponentType = componentType;
            ParentComponentType = parentComponentType;
            DeclaringAttributes = declaringAttributes;
            ComponentAttributes = componentAttributes;
            ParentComponentAttributes = parentComponentAttributes;
            AssemblyAttributes = assemblyAttributes;

            GlobalAttributes = ParentComponentAttributes.Concat(AssemblyAttributes).ToArray();
            AllAttributes = DeclaringAttributes.Concat(GlobalAttributes).Concat(ComponentAttributes).ToArray();
        }

        public string Name { get; private set; }

        public Type ComponentType { get; private set; }

        public Type ParentComponentType { get; private set; }

        public Attribute[] DeclaringAttributes { get; private set; }

        public Attribute[] ComponentAttributes { get; private set; }

        public Attribute[] ParentComponentAttributes { get; private set; }

        public Attribute[] AssemblyAttributes { get; private set; }

        public Attribute[] GlobalAttributes { get; private set; }

        public Attribute[] AllAttributes { get; private set; }

        public UIComponentDefinitionAttribute ComponentDefinitonAttribute { get; internal set; }

        public T GetFirstOrDefaultDeclaringAttribute<T>(Func<T, bool> predicate = null)
            where T : Attribute
        {
            return GetFirstOrDefaultAttribute(DeclaringAttributes, predicate);
        }

        public T GetFirstOrDefaultGlobalAttribute<T>(Func<T, bool> predicate = null)
            where T : Attribute
        {
            return GetFirstOrDefaultAttribute(GlobalAttributes, predicate);
        }

        public T GetFirstOrDefaultAssemblyAttribute<T>(Func<T, bool> predicate = null)
            where T : Attribute
        {
            return GetFirstOrDefaultAttribute(AssemblyAttributes, predicate);
        }

        public T GetFirstOrDefaultComponentAttribute<T>(Func<T, bool> predicate = null)
            where T : Attribute
        {
            return GetFirstOrDefaultAttribute(ComponentAttributes, predicate);
        }

        public T GetFirstOrDefaultAttribute<T>(Func<T, bool> predicate = null)
            where T : Attribute
        {
            return GetFirstOrDefaultAttribute(AllAttributes, predicate);
        }

        public T GetFirstOrDefaultDeclaringOrComponentAttribute<T>(Func<T, bool> predicate = null)
            where T : Attribute
        {
            return GetFirstOrDefaultAttribute(DeclaringAttributes.Concat(ComponentAttributes), predicate);
        }

        private T GetFirstOrDefaultAttribute<T>(IEnumerable<Attribute> attributes, Func<T, bool> predicate = null)
            where T : Attribute
        {
            var query = attributes.OfType<T>();
            return predicate == null ? query.FirstOrDefault() : query.FirstOrDefault(predicate);
        }

        public TermAttribute GetTerm(Func<TermAttribute, bool> predicate = null)
        {
            return GetFirstOrDefaultDeclaringAttribute<TermAttribute>(predicate);
        }

        public CultureInfo GetCulture()
        {
            return (GetFirstOrDefaultAttribute<CultureAttribute>(x => x.HasValue) ?? new CultureAttribute()).GetCultureInfo();
        }

        public string GetFormat()
        {
            return GetFirstOrDefaultDeclaringAttribute<FormatAttribute>()?.Value
                ?? GetFirstOrDefaultGlobalAttribute<FormatSettingsAttribute>(x => ComponentType.IsSubclassOfRawGeneric(x.ControlType))?.Value
                ?? GetFirstOrDefaultComponentAttribute<FormatAttribute>()?.Value;
        }
    }
}
