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

        // TODO: Review InitPageObjectTriggers method.
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
            UIComponentMetadata metadata = CreateComponentMetadata(property);

            UIComponent<TOwner> component = CreateComponent(parentComponent, metadata);

            property.SetValue(parentComponent, component);
        }

        private static void InitDelegateProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            // TODO: Implement InitDelegateProperty method.
        }

        public static TComponent CreateComponent<TComponent, TOwner>(UIComponent<TOwner> parentComponent, string name, params Attribute[] attributes)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            if (!attributes.OfType<NameAttribute>().Any())
                attributes = attributes.Concat(new[]
                    {
                        new NameAttribute(name)
                    }).ToArray();

            UIComponentMetadata metadata = CreateComponentMetadata(
                name,
                typeof(TComponent),
                parentComponent.GetType(),
                attributes);

            return (TComponent)CreateComponent<TOwner>(parentComponent, metadata);
        }

        private static UIComponent<TOwner> CreateComponent<TOwner>(UIComponent<TOwner> parentComponent, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            UIComponent<TOwner> component = (UIComponent<TOwner>)Activator.CreateInstance(metadata.ComponentType);

            InitComponent(component, parentComponent, metadata);
            UIComponentResolver.Resolve<TOwner>(component);

            return component;
        }

        private static void InitComponent<TOwner>(UIComponent<TOwner> component, UIComponent<TOwner> parentComponent, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            component.Owner = parentComponent.Owner ?? (TOwner)parentComponent;
            component.Parent = parentComponent;
            component.Log = parentComponent.Log;
            component.Driver = parentComponent.Driver;

            FindAttribute findAttribute = GetPropertyFindAttribute(metadata);

            InitComponentFinders(component, parentComponent, metadata, findAttribute);
            component.ComponentName = ResolveComponentName(metadata, findAttribute);
            component.CacheScopeElement = false;
            component.Triggers = GetControlTriggers(metadata);

            component.ApplyMetadata(metadata);
        }

        private static void InitComponentFinders(UIComponent component, UIComponent parentComponent, UIComponentMetadata metadata, FindAttribute findAttribute)
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

        private static string ResolveComponentName(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            NameAttribute nameAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<NameAttribute>();
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
                    TermAttribute termAttribute = metadata.GetTerm();
                    if (termAttribute != null && termAttribute.Values != null && termAttribute.Values.Any())
                        return string.Join("/", termAttribute.Values);
                }
            }

            return metadata.Name.Humanize(LetterCasing.Title);
        }

        private static UIComponentMetadata CreateComponentMetadata(PropertyInfo property)
        {
            return CreateComponentMetadata(
                property.Name,
                property.PropertyType,
                property.DeclaringType,
                GetPropertyAttributes(property));
        }

        private static UIComponentMetadata CreateComponentMetadata(string name, Type componentType, Type parentComponentType, Attribute[] declaringAttributes)
        {
            return new UIComponentMetadata(
                name,
                componentType,
                parentComponentType,
                GetComponentAttribute(componentType),
                declaringAttributes,
                GetClassAttributes(componentType),
                GetClassAttributes(parentComponentType),
                GetAssemblyAttributes(parentComponentType.Assembly));
        }

        private static FindAttribute GetPropertyFindAttribute(UIComponentMetadata metadata)
        {
            FindAttribute findAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<FindAttribute>();
            if (findAttribute != null)
            {
                return findAttribute;
            }
            else if (metadata.ComponentType.IsSubclassOfRawGeneric(typeof(EditableField<,>)))
            {
                var generalFindAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindEditableFieldsAttribute>();
                return generalFindAttribute != null ? generalFindAttribute.CreateFindAttribute() : FindEditableFieldsAttribute.CreateDefaultFindAttribute();
            }
            else if (metadata.ComponentType.IsSubclassOfRawGeneric(typeof(ClickableBase<,>)))
            {
                var generalFindAttribute = metadata.GetFirstOrDefaultGlobalAttribute<FindClickablesAttribute>();
                return generalFindAttribute != null ? generalFindAttribute.CreateFindAttribute() : FindClickablesAttribute.CreateDefaultFindAttribute();
            }
            else if (metadata.ComponentType.IsSubclassOfRawGeneric(typeof(Text<>)) && metadata.ParentComponentType.IsSubclassOfRawGeneric(typeof(TableRowBase<>)))
            {
                return new FindByColumnAttribute();
            }
            else
            {
                return new FindByIndexAttribute();
            }
        }

        private static FindItemAttribute GetPropertyFindItemAttribute(UIComponentMetadata metadata)
        {
            FindItemAttribute findAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<FindItemAttribute>();
            if (findAttribute != null)
            {
                return findAttribute;
            }
            else
            {
                return new FindItemByLabelAttribute();
            }
        }

        private static ElementFindOptions CreateFindOptions(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ElementFindOptions options = new ElementFindOptions
            {
                ElementXPath = metadata.ComponentAttribute.ElementXPath,
                IdFinderFormat = metadata.ComponentAttribute.IdFinderFormat,
                Index = findAttribute.Index != 0 ? (int?)findAttribute.Index : null
            };

            ITermFindAttribute termFindAttribute = findAttribute as ITermFindAttribute;
            ITermMatchFindAttribute termMatchFindAttribute = findAttribute as ITermMatchFindAttribute;

            if (termFindAttribute != null)
                options.Terms = termFindAttribute.GetTerms(metadata);

            if (termMatchFindAttribute != null)
                options.Match = termMatchFindAttribute.GetTermMatch(metadata);

            return options;
        }

        private static TriggerAttribute[] GetControlTriggers(UIComponentMetadata metadata)
        {
            List<TriggerAttribute> allTriggers = metadata.AllAttributes.OfType<TriggerAttribute>().ToList();
            List<TriggerAttribute> resultTriggers = new List<TriggerAttribute>();

            while (allTriggers.Count > 0)
            {
                Type currentTriggerType = allTriggers[0].GetType();
                TriggerAttribute[] currentTriggersOfSameType = allTriggers.Where(x => x.GetType() == currentTriggerType).ToArray();

                if (currentTriggersOfSameType.First().On != TriggerEvent.None)
                    resultTriggers.Add(currentTriggersOfSameType.First());

                foreach (TriggerAttribute trigger in currentTriggersOfSameType)
                    allTriggers.Remove(trigger);
            }

            return resultTriggers.OrderBy(x => x.Priority).ToArray();
        }

        private static UIComponentAttribute GetComponentAttribute(Type componentType)
        {
            // TODO: Add cache.
            UIComponentAttribute componentAttribute = componentType.GetCustomAttribute<UIComponentAttribute>(true);
            if (componentAttribute == null)
                throw new InvalidOperationException(string.Format("UIComponentAttribute is missing in '{0}' type", componentType.FullName));
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
