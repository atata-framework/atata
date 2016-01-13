using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public class UIPropertyMetadata
    {
        public UIPropertyMetadata(
            PropertyInfo property,
            UIComponentAttribute componentAttribute,
            Attribute[] propertyAttributes,
            Attribute[] classAttributes,
            Attribute[] assemblyAttributes)
        {
            Property = property;
            ComponentAttribute = componentAttribute;
            PropertyAttributes = propertyAttributes;
            ClassAttributes = classAttributes;
            AssemblyAttributes = assemblyAttributes;

            GlobalAttributes = ClassAttributes.Concat(AssemblyAttributes).ToArray();
        }

        public PropertyInfo Property { get; private set; }
        public UIComponentAttribute ComponentAttribute { get; private set; }
        public Attribute[] PropertyAttributes { get; private set; }
        public Attribute[] ClassAttributes { get; private set; }
        public Attribute[] AssemblyAttributes { get; private set; }
        public Attribute[] GlobalAttributes { get; private set; }

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
            return PropertyAttributes.
                Concat(GlobalAttributes).
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
