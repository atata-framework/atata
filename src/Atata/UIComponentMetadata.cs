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
            Attribute[] propertyAttributes,
            Attribute[] classAttributes,
            Attribute[] assemblyAttributes)
        {
            Name = name;
            ComponentType = componentType;
            ParentComponentType = parentComponentType;
            ComponentAttribute = componentAttribute;
            PropertyAttributes = propertyAttributes;
            ClassAttributes = classAttributes;
            AssemblyAttributes = assemblyAttributes;

            GlobalAttributes = ClassAttributes.Concat(AssemblyAttributes).ToArray();
            AllAttributes = PropertyAttributes.Concat(GlobalAttributes).ToArray();
        }

        public string Name { get; private set; }
        public Type ComponentType { get; private set; }
        public Type ParentComponentType { get; private set; }
        public UIComponentAttribute ComponentAttribute { get; private set; }
        public Attribute[] PropertyAttributes { get; private set; }
        public Attribute[] ClassAttributes { get; private set; }
        public Attribute[] AssemblyAttributes { get; private set; }
        public Attribute[] GlobalAttributes { get; private set; }
        public Attribute[] AllAttributes { get; private set; }

        public T GetFirstOrDefaultPropertyAttribute<T>() where T : Attribute
        {
            return PropertyAttributes.
                OfType<T>().
                FirstOrDefault();
        }

        public T GetFirstOrDefaultPropertyAttribute<T>(Func<T, bool> predicate) where T : Attribute
        {
            return PropertyAttributes.
                OfType<T>().
                FirstOrDefault(predicate);
        }

        public T GetFirstOrDefaultGlobalAttribute<T>() where T : Attribute
        {
            return GlobalAttributes.
                OfType<T>().
                FirstOrDefault();
        }

        public T GetFirstOrDefaultGlobalAttribute<T>(Func<T, bool> predicate) where T : Attribute
        {
            return GlobalAttributes.
                OfType<T>().
                FirstOrDefault(predicate);
        }

        public T GetFirstOrDefaultAttribute<T>(Func<T, bool> predicate) where T : Attribute
        {
            return AllAttributes.
                OfType<T>().
                FirstOrDefault(predicate);
        }

        public CultureInfo GetCulture()
        {
            return (GetFirstOrDefaultAttribute<CultureAttribute>(x => x.HasValue) ?? new CultureAttribute()).GetCultureInfo();
        }

        public string GetFormat(Type componentType)
        {
            FormatAttribute formatAttribute = GetFirstOrDefaultPropertyAttribute<FormatAttribute>();
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
