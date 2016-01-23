using System;
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
            UIComponentAttribute componentAttribute,
            Attribute[] declaringAttributes,
            Attribute[] componentAttributes,
            Attribute[] parentComponentAttributes,
            Attribute[] assemblyAttributes)
        {
            Name = name;
            ComponentType = componentType;
            ParentComponentType = parentComponentType;
            ComponentAttribute = componentAttribute;
            DeclaringAttributes = declaringAttributes;
            ComponentAttributes = componentAttributes;
            ParentComponentAttributes = parentComponentAttributes;
            AssemblyAttributes = assemblyAttributes;

            GlobalAttributes = ParentComponentAttributes.Concat(AssemblyAttributes).ToArray();
            AllAttributes = DeclaringAttributes.Concat(GlobalAttributes).ToArray();
        }

        public string Name { get; private set; }
        public Type ComponentType { get; private set; }
        public Type ParentComponentType { get; private set; }
        public UIComponentAttribute ComponentAttribute { get; private set; }
        public Attribute[] DeclaringAttributes { get; private set; }
        public Attribute[] ComponentAttributes { get; private set; }
        public Attribute[] ParentComponentAttributes { get; private set; }
        public Attribute[] AssemblyAttributes { get; private set; }
        public Attribute[] GlobalAttributes { get; private set; }
        public Attribute[] AllAttributes { get; private set; }

        public T GetFirstOrDefaultDeclaringAttribute<T>(Func<T, bool> predicate = null) where T : Attribute
        {
            return GetFirstOrDefaultAttribute(DeclaringAttributes, predicate);
        }

        public T GetFirstOrDefaultGlobalAttribute<T>(Func<T, bool> predicate = null) where T : Attribute
        {
            return GetFirstOrDefaultAttribute(GlobalAttributes, predicate);
        }

        public T GetFirstOrDefaultAttribute<T>(Func<T, bool> predicate = null) where T : Attribute
        {
            return GetFirstOrDefaultAttribute(AllAttributes, predicate);
        }

        private T GetFirstOrDefaultAttribute<T>(Attribute[] attributes, Func<T, bool> predicate = null) where T : Attribute
        {
            var query = attributes.OfType<T>();
            return predicate == null ? query.FirstOrDefault() : query.FirstOrDefault(predicate);
        }

        public TermAttribute GetTerm()
        {
            return GetFirstOrDefaultDeclaringAttribute<TermAttribute>();
        }

        public TermAttribute GetTerm(Func<TermAttribute, bool> predicate)
        {
            return GetFirstOrDefaultDeclaringAttribute<TermAttribute>(predicate);
        }

        public CultureInfo GetCulture()
        {
            return (GetFirstOrDefaultAttribute<CultureAttribute>(x => x.HasValue) ?? new CultureAttribute()).GetCultureInfo();
        }

        public string GetFormat(Type componentType)
        {
            FormatAttribute formatAttribute = GetFirstOrDefaultDeclaringAttribute<FormatAttribute>();
            if (formatAttribute != null)
            {
                return formatAttribute.Value;
            }
            else
            {
                UIComponentFormatAttribute componentFormatAttribute =
                    GetFirstOrDefaultGlobalAttribute<UIComponentFormatAttribute>(x => componentType.IsSubclassOfRawGeneric(x.ComponentType));
                if (componentFormatAttribute != null)
                    return componentFormatAttribute.Value;
                else
                    return null;
            }
        }
    }
}
