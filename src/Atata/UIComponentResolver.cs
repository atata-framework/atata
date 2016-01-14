using Humanizer;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class UIComponentResolver
    {
        private static readonly Dictionary<PropertyInfo, Attribute[]> PropertyAttributes = new Dictionary<PropertyInfo, Attribute[]>();
        private static readonly Dictionary<Type, Attribute[]> ClassAttributes = new Dictionary<Type, Attribute[]>();
        private static readonly Dictionary<Assembly, Attribute[]> AssemblyAttributes = new Dictionary<Assembly, Attribute[]>();

        private static readonly Dictionary<Type, string> PageObjectNames = new Dictionary<Type, string>();

        public static void Resolve<TOwner>(UIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            PageObject<TOwner> componentAsPageObject = component as PageObject<TOwner>;
            if (componentAsPageObject != null)
                InitPageObjectTriggers(componentAsPageObject);

            Type[] allTypes = GetAllInheritedTypes(component.GetType()).Reverse().ToArray();

            foreach (Type type in allTypes)
                Init<TOwner>(component, type);
        }

        public static void InitPageObjectTriggers<TOwner>(PageObject<TOwner> pageObject)
            where TOwner : PageObject<TOwner>
        {
            Type pageObjectType = pageObject.GetType();
            pageObject.Triggers = GetClassAttributes(pageObjectType).OfType<TriggerAttribute>().ToArray();
        }

        private static IEnumerable<Type> GetAllInheritedTypes(Type type)
        {
            Type typeToCheck = type;
            while (type != typeof(UIComponent) && (!type.IsGenericType || (type.GetGenericTypeDefinition() != typeof(UIComponent<>) && type.GetGenericTypeDefinition() != typeof(PageObject<>))))
            {
                yield return type;
                type = type.BaseType;
            }
        }

        private static void Init<TOwner>(UIComponent<TOwner> component, Type type)
            where TOwner : PageObject<TOwner>
        {
            PropertyInfo[] suitableProperties = type.
                GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.SetProperty).
                Where(x => x.GetCustomAttribute<IgnoreInitAttribute>() == null).
                ToArray();

            PropertyInfo[] controlProperties = suitableProperties.
                Where(x => x.PropertyType.IsSubclassOfRawGeneric(typeof(Control<>))).
                ToArray();

            foreach (var property in controlProperties)
                InitControlProperty<TOwner>(component, property);

            PropertyInfo[] delegateProperties = suitableProperties.
                Where(x => x.PropertyType.IsSubclassOfRawGeneric(typeof(Func<>))).
                ToArray();

            foreach (var property in delegateProperties)
                InitDelegateProperty<TOwner>(component, property);
        }

        private static void InitControlProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            Control<TOwner> control = (Control<TOwner>)Activator.CreateInstance(property.PropertyType);
            InitComponent(control, parentComponent, property);

            property.SetValue(parentComponent, control);

            UIComponentResolver.Resolve<TOwner>(control);
        }

        private static void InitDelegateProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            // TODO: Implement InitDelegateProperty method.
        }

        private static void InitComponent<TOwner>(UIComponent<TOwner> component, UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            component.Owner = parentComponent.Owner ?? (TOwner)parentComponent;
            component.Parent = parentComponent;
            component.Log = parentComponent.Log;
            component.Driver = parentComponent.Driver;

            UIPropertyMetadata metadata = CreatePropertyMetadata(property);
            FindAttribute findAttribute = GetPropertyFindAttribute(metadata);

            InitComponentFinders(component, parentComponent, metadata, findAttribute);
            component.ComponentName = ResolveComponentName(metadata, findAttribute);
            component.CacheScopeElement = false;
            component.Triggers = GetControlTriggers(metadata);

            component.ApplyMetadata(metadata);
        }

        private static void InitComponentFinders(UIComponent component, UIComponent parentComponent, UIPropertyMetadata metadata, FindAttribute findAttribute)
        {
            ElementFindOptions findOptions = CreateFindOptions(metadata, findAttribute);
            IElementFindStrategy strategy = findAttribute.CreateStrategy(metadata);

            IItemsControl itemsControl = component as IItemsControl;

            ScopeSource scopeSource = findAttribute.GetScope(metadata);

            if (itemsControl != null)
            {
                component.ScopeElementFinder = isSafely =>
                    {
                        return GetScopeElement(parentComponent, scopeSource);
                    };
                itemsControl.ItemsFindStrategy = strategy;
                itemsControl.ItemsFindOptions = findOptions;
                FindItemAttribute findItemAttribute = GetPropertyFindItemAttribute(metadata);
                itemsControl.ItemFindStrategy = findItemAttribute.CreateStrategy(metadata);
            }
            else
            {
                component.ScopeElementFinder = isSafely =>
                    {
                        findOptions.IsSafely = isSafely;
                        IWebElement scope = GetScopeElement(parentComponent, scopeSource);
                        ElementLocator locator = strategy.Find(scope, findOptions);
                        return locator.GetElement(isSafely);
                    };
            }
        }

        private static IWebElement GetScopeElement(UIComponent parentComponent, ScopeSource scopeSource)
        {
            switch (scopeSource)
            {
                case ScopeSource.Parent:
                    return parentComponent.Scope;
                case ScopeSource.Grandparent:
                    return parentComponent.Parent.Scope;
                case ScopeSource.PageObject:
                    return parentComponent.Owner.Scope;
                case ScopeSource.Page:
                    return parentComponent.Driver.Get(By.TagName("body"));
                default:
                    throw new ArgumentException("scopeSource", "Unsupported '{0}' value of ScopeSource".FormatWith(scopeSource));
            }
        }

        private static string ResolveComponentName(UIPropertyMetadata metadata, FindAttribute findAttribute)
        {
            NameAttribute nameAttribute = metadata.GetFirstOrDefaultPropertyAttribute<NameAttribute>();
            if (nameAttribute != null)
            {
                return nameAttribute.Value;
            }

            FindByLabelAttribute findByLabelAttribute = findAttribute as FindByLabelAttribute;
            if (findByLabelAttribute != null)
            {
                if (findByLabelAttribute.Values != null && findByLabelAttribute.Values.Any())
                {
                    return string.Join("/", findByLabelAttribute.Values);
                }
                else
                {
                    StringValueAttribute stringValueAttribute = metadata.GetFirstOrDefaultPropertyAttribute<StringValueAttribute>();
                    if (stringValueAttribute != null && stringValueAttribute.Values != null && stringValueAttribute.Values.Any())
                        return string.Join("/", stringValueAttribute.Values);
                }
            }

            return metadata.Property.Name.Humanize(LetterCasing.Title);
        }

        private static UIPropertyMetadata CreatePropertyMetadata(PropertyInfo property)
        {
            return new UIPropertyMetadata(
                property,
                GetComponentAttribute(property),
                GetPropertyAttributes(property),
                GetClassAttributes(property.DeclaringType),
                GetAssemblyAttributes(property.DeclaringType.Assembly));
        }

        private static FindAttribute GetPropertyFindAttribute(UIPropertyMetadata metadata)
        {
            FindAttribute findAttribute = metadata.GetFirstOrDefaultPropertyAttribute<FindAttribute>();
            if (findAttribute != null)
            {
                return findAttribute;
            }
            else if (metadata.Property.PropertyType.IsSubclassOfRawGeneric(typeof(EditableField<,>)))
            {
                var generalFindAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindEditableFieldsAttribute>();
                return generalFindAttribute != null ? generalFindAttribute.CreateFindAttribute() : FindEditableFieldsAttribute.CreateDefaultFindAttribute();
            }
            else if (metadata.Property.PropertyType.IsSubclassOfRawGeneric(typeof(ClickableBase<,>)))
            {
                var generalFindAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindClickablesAttribute>();
                return generalFindAttribute != null ? generalFindAttribute.CreateFindAttribute() : FindClickablesAttribute.CreateDefaultFindAttribute();
            }
            else if (metadata.Property.PropertyType.IsSubclassOfRawGeneric(typeof(Text<>)) && metadata.Property.DeclaringType.IsSubclassOfRawGeneric(typeof(TableRowBase<>)))
            {
                return new FindByColumnAttribute();
            }
            else
            {
                return new FindByIndexAttribute();
            }
        }

        private static FindItemAttribute GetPropertyFindItemAttribute(UIPropertyMetadata metadata)
        {
            FindItemAttribute findAttribute = metadata.GetFirstOrDefaultPropertyAttribute<FindItemAttribute>();
            if (findAttribute != null)
            {
                return findAttribute;
            }
            else
            {
                return new FindItemByLabelAttribute();
            }
        }

        private static ElementFindOptions CreateFindOptions(UIPropertyMetadata metadata, FindAttribute findAttribute)
        {
            ElementFindOptions options = new ElementFindOptions
            {
                ElementXPath = metadata.ComponentAttribute.ElementXPath,
                IdFinderFormat = metadata.ComponentAttribute.IdFinderFormat,
                Index = findAttribute.Index != 0 ? (int?)findAttribute.Index : null
            };

            IQualifierAttribute qualifierAttribute = findAttribute as IQualifierAttribute;
            IQualifierMatchAttribute qualifierMatchAttribute = findAttribute as IQualifierMatchAttribute;

            if (qualifierAttribute != null)
                options.Qualifiers = qualifierAttribute.GetQualifiers(metadata);

            if (qualifierMatchAttribute != null)
                options.Match = qualifierMatchAttribute.GetQualifierMatch(metadata);

            return options;
        }

        private static TriggerAttribute[] GetControlTriggers(UIPropertyMetadata metadata)
        {
            return metadata.PropertyAttributes.OfType<TriggerAttribute>().ToArray();
        }

        private static UIComponentAttribute GetComponentAttribute(PropertyInfo property)
        {
            // TODO: Add cache.
            UIComponentAttribute componentAttribute = property.PropertyType.GetCustomAttribute<UIComponentAttribute>(true);
            if (componentAttribute == null)
                throw new InvalidOperationException(string.Format("UIComponentAttribute is missing in '{0}' type", property.PropertyType.FullName));
            return componentAttribute;
        }

        private static Attribute[] GetPropertyAttributes(PropertyInfo property)
        {
            Attribute[] attributes;
            if (PropertyAttributes.TryGetValue(property, out attributes))
                return attributes;
            return PropertyAttributes[property] = property.GetCustomAttributes(true).Cast<Attribute>().ToArray();
        }

        private static Attribute[] GetClassAttributes(Type type)
        {
            Attribute[] attributes;
            if (ClassAttributes.TryGetValue(type, out attributes))
                return attributes;
            return ClassAttributes[type] = type.GetCustomAttributes(true).Cast<Attribute>().ToArray();
        }

        private static Attribute[] GetAssemblyAttributes(Assembly assembly)
        {
            Attribute[] attributes;
            if (AssemblyAttributes.TryGetValue(assembly, out attributes))
                return attributes;
            return AssemblyAttributes[assembly] = assembly.GetCustomAttributes(true).Cast<Attribute>().ToArray();
        }

        public static string ResolvePageObjectName<TPageObject>()
            where TPageObject : PageObject<TPageObject>
        {
            Type type = typeof(TPageObject);
            string name;
            if (PageObjectNames.TryGetValue(type, out name))
                return name;
            return PageObjectNames[type] = ResolvePageObjectNameFromMetadata(type);
        }

        private static string ResolvePageObjectNameFromMetadata(Type type)
        {
            NameAttribute nameAttribute = GetClassAttributes(type).OfType<NameAttribute>().FirstOrDefault();
            string name = nameAttribute != null && !string.IsNullOrWhiteSpace(nameAttribute.Value)
                ? nameAttribute.Value
                : ResolvePageObjectNameFromTypeName(type.Name);

            return AddEndingToPageObjectName(type, name);
        }

        private static string ResolvePageObjectNameFromTypeName(string typeName)
        {
            string[] endingsToIgnore = { "Page", "Window" };
            string foundEndingToIgnore = endingsToIgnore.FirstOrDefault(x => typeName.EndsWith(x));

            string name = foundEndingToIgnore != null
                ? typeName.Substring(0, typeName.Length - foundEndingToIgnore.Length)
                : typeName;
            return name.Humanize(LetterCasing.Title);
        }

        private static string AddEndingToPageObjectName(Type type, string name)
        {
            string ending = type.IsSubclassOfRawGeneric(typeof(Page<>))
                ? "page"
                : type.IsSubclassOfRawGeneric(typeof(PopupWindow<>))
                ? "window"
                : null;
            return ending != null ? string.Format("{0} {1}", name, ending) : name;
        }
    }
}
