using Humanizer;
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
            Type[] allTypes = GetAllInheritedTypes(component.GetType()).Reverse().ToArray();

            foreach (Type type in allTypes)
                InitComponentTypeMembers<TOwner>(component, type);

            PageObject<TOwner> componentAsPageObject = component as PageObject<TOwner>;
            if (componentAsPageObject != null)
                InitPageObjectTriggers(componentAsPageObject);
        }

        // TODO: Review InitPageObjectTriggers method.
        public static void InitPageObjectTriggers<TOwner>(PageObject<TOwner> pageObject)
            where TOwner : PageObject<TOwner>
        {
            Type pageObjectType = pageObject.GetType();
            pageObject.Triggers = GetClassAttributes(pageObjectType).
                Concat(GetAssemblyAttributes(pageObjectType.Assembly)).
                OfType<TriggerAttribute>().
                ToArray();
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

        private static void InitComponentTypeMembers<TOwner>(UIComponent<TOwner> component, Type type)
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

            property.SetValue(parentComponent, component, null);
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

            InitComponentLocator(component, parentComponent, metadata, findAttribute);
            component.ComponentName = ResolveComponentName(metadata, findAttribute);
            component.CacheScopeElement = false;
            component.Triggers = GetControlTriggers(metadata);

            component.ApplyMetadata(metadata);
        }

        private static void InitComponentLocator(UIComponent component, UIComponent parentComponent, UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ComponentScopeLocateOptions findOptions = CreateFindOptions(metadata, findAttribute);
            IComponentScopeLocateStrategy elementLocator = findAttribute.CreateStrategy(metadata);

            IItemsControl itemsControl = component as IItemsControl;

            component.ScopeSource = findAttribute.GetScope(metadata);

            if (itemsControl != null)
            {
                ////component.ScopeElementFinder = isSafely =>
                ////    {
                ////        return component.ScopeSource.GetScopeElement(parentComponent);
                ////    };
                ////itemsControl.ItemsFindStrategy = elementLocator;
                ////itemsControl.ItemsFindOptions = findOptions;
                ////FindItemAttribute findItemAttribute = GetPropertyFindItemAttribute(metadata);
                ////itemsControl.ItemFindStrategy = findItemAttribute.CreateStrategy(metadata);
            }
            else
            {
                ////component.ScopeElementFinder = isSafely =>
                ////    {
                ////        findOptions.IsSafely = isSafely;
                ////        IWebElement scope = component.ScopeSource.GetScopeElement(parentComponent);
                ////        ElementLocator locator = elementLocator.Find(scope, findOptions, null);
                ////        return locator.GetElement(isSafely);
                ////    };
            }
            component.ScopeLocator = new StrategyScopeLocator(component, elementLocator, findOptions);
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
            else
            {
                Type controlType = metadata.ComponentType;
                Type parentControlType = metadata.ParentComponentType;

                FindControlsAttribute findControlsAttribute =
                    GetNearestFindControlsAttribute(controlType, parentControlType, metadata.ParentComponentAttributes) ??
                    GetNearestFindControlsAttribute(controlType, parentControlType, metadata.AssemblyAttributes);

                return findControlsAttribute != null
                    ? findControlsAttribute.CreateFindAttribute()
                    : GetDefaultFindAttribute(controlType, parentControlType);
            }
        }

        private static FindControlsAttribute GetNearestFindControlsAttribute(Type controlType, Type parentControlType, IEnumerable<Attribute> attributes)
        {
            return attributes.OfType<FindControlsAttribute>().
                Select(attr => new { Attribute = attr, Depth = controlType.GetDepthOfInheritanceOfRawGeneric(attr.ControlType) }).
                Where(x => x.Depth != null).
                OrderBy(x => x.Depth).
                Select(x => x.Attribute).
                FirstOrDefault(attr => attr.ParentControlType == null || parentControlType.IsSubclassOfRawGeneric(attr.ParentControlType));
        }

        // TODO: Remove GetDefaultFindAttribute method. Move this logic to some other place.
        private static FindAttribute GetDefaultFindAttribute(Type controlType, Type parentControlType)
        {
            if (controlType.IsSubclassOfRawGeneric(typeof(Field<,>)))
                return new FindByLabelAttribute();
            else if (controlType.IsSubclassOfRawGeneric(typeof(Link<>)))
                return new FindByContentAttribute();
            else if (controlType.IsSubclassOfRawGeneric(typeof(Clickable<>)))
                return new FindByContentOrValueAttribute();
            else if (controlType.IsSubclassOfRawGeneric(typeof(Text<>)) && parentControlType.IsSubclassOfRawGeneric(typeof(TableRowBase<>)))
                return new FindByColumnAttribute();
            else
                return new FindByIndexAttribute();
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

        private static ComponentScopeLocateOptions CreateFindOptions(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ComponentScopeLocateOptions options = new ComponentScopeLocateOptions
            {
                ElementXPath = metadata.ComponentDefinitonAttribute != null ? metadata.ComponentDefinitonAttribute.ElementXPath : "*",
                IdFinderFormat = metadata.ComponentDefinitonAttribute != null ? metadata.ComponentDefinitonAttribute.IdFinderFormat : null,
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
            List<TriggerAttribute> resultTriggers = metadata.ComponentAttributes.
                OfType<TriggerAttribute>().
                Where(x => x.AppliesTo == TriggerScope.Self).
                OrderBy(x => x.Priority).
                ToList();

            List<TriggerAttribute> allOtherTriggers = metadata.DeclaringAttributes.
                Concat(metadata.ParentComponentAttributes.OfType<TriggerAttribute>().Where(x => x.AppliesTo == TriggerScope.Children)).
                Concat(metadata.AssemblyAttributes).
                OfType<TriggerAttribute>().
                ToList();

            while (allOtherTriggers.Count > 0)
            {
                Type currentTriggerType = allOtherTriggers[0].GetType();
                TriggerAttribute[] currentTriggersOfSameType = allOtherTriggers.Where(x => x.GetType() == currentTriggerType).ToArray();

                if (currentTriggersOfSameType.First().On != TriggerEvents.None)
                    resultTriggers.Add(currentTriggersOfSameType.First());

                foreach (TriggerAttribute trigger in currentTriggersOfSameType)
                    allOtherTriggers.Remove(trigger);
            }

            return resultTriggers.OrderBy(x => x.Priority).ToArray();
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
            string normalizedTypeName = typeName.Contains("`")
                ? typeName.Substring(0, typeName.IndexOf('`'))
                : typeName;

            string[] endingsToIgnore = { "Page", "Window" };
            string foundEndingToIgnore = endingsToIgnore.FirstOrDefault(x => normalizedTypeName.EndsWith(x));

            string name = foundEndingToIgnore != null
                ? normalizedTypeName.Substring(0, normalizedTypeName.Length - foundEndingToIgnore.Length)
                : normalizedTypeName;
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
