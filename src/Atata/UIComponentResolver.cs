using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class UIComponentResolver
    {
        private static readonly Dictionary<ICustomAttributeProvider, Attribute[]> PropertyAttributes;
        private static readonly Dictionary<ICustomAttributeProvider, Attribute[]> ClassAttributes;
        private static readonly Dictionary<ICustomAttributeProvider, Attribute[]> AssemblyAttributes;

        private static readonly Dictionary<Type, string> PageObjectNames;
        private static readonly Dictionary<Type, Type> DelegateControlsTypeMapping;
        private static readonly Dictionary<Delegate, UIComponent> DelegateControls;

        static UIComponentResolver()
        {
            PropertyAttributes = new Dictionary<ICustomAttributeProvider, Attribute[]>();
            ClassAttributes = new Dictionary<ICustomAttributeProvider, Attribute[]>();
            AssemblyAttributes = new Dictionary<ICustomAttributeProvider, Attribute[]>();

            PageObjectNames = new Dictionary<Type, string>();
            DelegateControlsTypeMapping = new Dictionary<Type, Type>();
            DelegateControls = new Dictionary<Delegate, UIComponent>();

            InitDelegateControlMappings();
        }

        private static void InitDelegateControlMappings()
        {
            RegisterDelegateControlMapping(typeof(Clickable<>), typeof(ClickableControl<>));
            RegisterDelegateControlMapping(typeof(Clickable<,>), typeof(ClickableControl<,>));

            RegisterDelegateControlMapping(typeof(Link<>), typeof(LinkControl<>));
            RegisterDelegateControlMapping(typeof(Link<,>), typeof(LinkControl<,>));

            RegisterDelegateControlMapping(typeof(Button<>), typeof(ButtonControl<>));
            RegisterDelegateControlMapping(typeof(Button<,>), typeof(ButtonControl<,>));
        }

        public static void RegisterDelegateControlMapping(Type delegateType, Type controlType)
        {
            DelegateControlsTypeMapping[delegateType] = controlType;
        }

        public static void Resolve<TOwner>(UIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            Type[] allTypes = GetAllInheritedTypes(component.GetType()).Reverse().ToArray();

            foreach (Type type in allTypes)
                InitComponentTypeMembers<TOwner>(component, type);

            PageObject<TOwner> componentAsPageObject = component as PageObject<TOwner>;
            if (componentAsPageObject != null)
            {
                InitPageObject(componentAsPageObject);
            }
        }

        private static void InitPageObject<TPageObject>(PageObject<TPageObject> pageObject)
            where TPageObject : PageObject<TPageObject>
        {
            pageObject.Owner = (TPageObject)pageObject;
            InitPageObjectTriggers(pageObject);

            UIComponentMetadata metadata = CreatePageObjectMetadata(pageObject.GetType());
            ApplyMetadata(pageObject, metadata);
        }

        // TODO: Review InitPageObjectTriggers method.
        private static void InitPageObjectTriggers<TOwner>(PageObject<TOwner> pageObject)
            where TOwner : PageObject<TOwner>
        {
            Type pageObjectType = pageObject.GetType();
            var classTriggers = GetClassAttributes(pageObjectType).OfType<TriggerAttribute>().Where(x => x.AppliesTo == TriggerScope.Self);
            var assemblyTriggers = GetAssemblyAttributes(pageObjectType.Assembly).OfType<TriggerAttribute>();

            pageObject.Triggers = classTriggers.
                Concat(assemblyTriggers).
                OrderBy(x => x.Priority).
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
                Where(x => typeof(MulticastDelegate).IsAssignableFrom(x.PropertyType.BaseType) && x.PropertyType.IsGenericType).
                ToArray();

            foreach (var property in delegateProperties)
                InitDelegateProperty<TOwner>(component, property);
        }

        private static void InitControlProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            UIComponentMetadata metadata = CreateComponentMetadata(property);

            UIComponent<TOwner> component = CreateComponent(parentComponent, metadata);
            parentComponent.Children.Add(component);

            property.SetValue(parentComponent, component, null);
        }

        private static void InitDelegateProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            Type controlType = ResolveDelegateControlType(property.PropertyType);

            if (controlType != null)
            {
                UIComponentMetadata metadata = CreateComponentMetadata(property, controlType);

                UIComponent<TOwner> component = CreateComponent(parentComponent, metadata);
                parentComponent.Children.Add(component);

                Delegate clickDelegate = Delegate.CreateDelegate(property.PropertyType, component, "Click");
                property.SetValue(parentComponent, clickDelegate, null);

                DelegateControls[clickDelegate] = component;
            }
        }

        private static Type ResolveDelegateControlType(Type delegateType)
        {
            Type delegateGenericTypeDefinition = delegateType.GetGenericTypeDefinition();
            Type controlGenericTypeDefinition;

            if (DelegateControlsTypeMapping.TryGetValue(delegateGenericTypeDefinition, out controlGenericTypeDefinition))
            {
                Type[] genericArguments = delegateType.GetGenericArguments();
                return controlGenericTypeDefinition.MakeGenericType(genericArguments);
            }
            return null;
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
                attributes,
                GetControlDefinition(typeof(TComponent)));

            return (TComponent)CreateComponent<TOwner>(parentComponent, metadata);
        }

        private static UIComponent<TOwner> CreateComponent<TOwner>(UIComponent<TOwner> parentComponent, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            UIComponent<TOwner> component = (UIComponent<TOwner>)ActivatorEx.CreateInstance(metadata.ComponentType);

            InitComponent(component, parentComponent, metadata);
            Resolve(component);

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
            component.ComponentTypeName = ResolveControlTypeName(component.GetType());
            component.CacheScopeElement = false;
            component.Triggers = GetControlTriggers(metadata);

            ApplyMetadata(component, metadata);
        }

        private static void ApplyMetadata<TOwner>(UIComponent<TOwner> component, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            component.Metadata = metadata;
            component.ApplyMetadata(metadata);

            foreach (TriggerAttribute trigger in component.Triggers)
            {
                trigger.ApplyMetadata(metadata);
            }
        }

        private static void InitComponentLocator(UIComponent component, UIComponent parentComponent, UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ComponentScopeLocateOptions findOptions = CreateFindOptions(metadata, findAttribute);
            IComponentScopeLocateStrategy elementLocator = findAttribute.CreateStrategy(metadata);

            IItemsControl itemsControl = component as IItemsControl;

            component.ScopeSource = findAttribute.GetScope(metadata);

            if (itemsControl != null)
            {
                FindItemAttribute findItemAttribute = GetPropertyFindItemAttribute(metadata);
                IItemElementFindStrategy itemElementFindStrategy = findItemAttribute.CreateStrategy(metadata);
                itemsControl.Apply(itemElementFindStrategy);
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

            return metadata.Name.ToString(TermFormat.Title);
        }

        private static UIComponentMetadata CreatePageObjectMetadata(Type type)
        {
            // TODO: Review name set.
            return CreateComponentMetadata(
                type.Name,
                type,
                null,
                new Attribute[0],
                GetPageObjectDefinition(type));
        }

        private static UIComponentMetadata CreateComponentMetadata(PropertyInfo property, Type propertyType = null)
        {
            return CreateComponentMetadata(
                property.Name,
                propertyType ?? property.PropertyType,
                property.DeclaringType,
                GetPropertyAttributes(property),
                GetControlDefinition(propertyType ?? property.PropertyType));
        }

        private static UIComponentMetadata CreateComponentMetadata(
            string name,
            Type componentType,
            Type parentComponentType,
            Attribute[] declaringAttributes,
            UIComponentDefinitionAttribute componentDefinitonAttribute)
        {
            return new UIComponentMetadata(
                name,
                componentType,
                parentComponentType,
                declaringAttributes,
                GetClassAttributes(componentType),
                GetClassAttributes(parentComponentType),
                GetAssemblyAttributes((parentComponentType != null ? parentComponentType : componentType).Assembly),
                componentDefinitonAttribute);
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
                Type parentComponentType = metadata.ParentComponentType;

                ControlFindingAttribute controlFindingAttribute =
                    GetNearestControlFindingAttribute(controlType, parentComponentType, metadata.ParentComponentAttributes) ??
                    GetNearestControlFindingAttribute(controlType, parentComponentType, metadata.AssemblyAttributes) ??
                    GetNearestDefaultControlFindingAttribute(parentComponentType, metadata.ComponentAttributes);

                return controlFindingAttribute != null
                    ? controlFindingAttribute.CreateFindAttribute()
                    : GetDefaultFindAttribute(controlType, parentComponentType, metadata);
            }
        }

        private static ControlFindingAttribute GetNearestControlFindingAttribute(Type controlType, Type parentComponentType, IEnumerable<Attribute> attributes)
        {
            return attributes.OfType<ControlFindingAttribute>().
                Select(attr => new { Attribute = attr, Depth = controlType.GetDepthOfInheritanceOfRawGeneric(attr.ControlType) }).
                Where(x => x.Depth != null).
                OrderBy(x => x.Depth).
                Select(x => x.Attribute).
                FirstOrDefault(attr => attr.ParentComponentType == null || parentComponentType.IsSubclassOfRawGeneric(attr.ParentComponentType));
        }

        private static ControlFindingAttribute GetNearestDefaultControlFindingAttribute(Type parentComponentType, IEnumerable<Attribute> attributes)
        {
            var allFindingAttributes = attributes.OfType<ControlFindingAttribute>().
                Where(x => x.ControlType == null).
                Select(attr => new { Attribute = attr, Depth = parentComponentType.GetDepthOfInheritanceOfRawGeneric(attr.ParentComponentType) }).
                ToArray();

            return allFindingAttributes.Where(x => x.Depth != null).OrderBy(x => x.Depth).Select(x => x.Attribute).FirstOrDefault() ??
                allFindingAttributes.Where(x => x.Depth == null).Select(x => x.Attribute).FirstOrDefault();
        }

        private static FindAttribute GetDefaultFindAttribute(Type controlType, Type parentControlType, UIComponentMetadata metadata)
        {
            if (metadata.ComponentDefinitonAttribute.ScopeXPath == "*")
                return new UseParentScopeAttribute();

            return new FindFirstAttribute();
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
            ControlDefinitionAttribute definition = metadata.ComponentDefinitonAttribute as ControlDefinitionAttribute;
            ComponentScopeLocateOptions options = new ComponentScopeLocateOptions
            {
                ElementXPath = definition != null ? definition.ScopeXPath : "*",
                IdFinderFormat = definition != null ? definition.IdFinderFormat : null,
                Index = findAttribute.IsIndexSet ? (int?)findAttribute.Index : null
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

        private static Attribute[] ResolveAndCacheAttributes(Dictionary<ICustomAttributeProvider, Attribute[]> cache, ICustomAttributeProvider attributeProvider)
        {
            if (attributeProvider == null)
                return new Attribute[0];

            Attribute[] attributes;

            if (cache.TryGetValue(attributeProvider, out attributes))
                return attributes;

            lock (cache)
            {
                if (cache.TryGetValue(attributeProvider, out attributes))
                    return attributes;
                else
                    return cache[attributeProvider] = attributeProvider.GetCustomAttributes(true).Cast<Attribute>().ToArray();
            }
        }

        private static Attribute[] GetPropertyAttributes(PropertyInfo property)
        {
            return ResolveAndCacheAttributes(PropertyAttributes, property);
        }

        private static Attribute[] GetClassAttributes(Type type)
        {
            return ResolveAndCacheAttributes(ClassAttributes, type);
        }

        private static Attribute[] GetAssemblyAttributes(Assembly assembly)
        {
            return ResolveAndCacheAttributes(AssemblyAttributes, assembly);
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
            return nameAttribute != null && !string.IsNullOrWhiteSpace(nameAttribute.Value)
                ? nameAttribute.Value
                : ResolvePageObjectNameFromType(type);
        }

        private static string ResolvePageObjectNameFromType(Type type)
        {
            string typeName = NormalizeTypeName(type);

            string[] endingsToIgnore = GetPageObjectDefinition(type).GetIgnoreNameEndingValues();
            string foundEndingToIgnore = endingsToIgnore.FirstOrDefault(x => typeName.EndsWith(x));

            string name = foundEndingToIgnore != null
                ? typeName.Substring(0, typeName.Length - foundEndingToIgnore.Length)
                : typeName;
            return name.ToString(TermFormat.Title);
        }

        public static string ResolvePageObjectTypeName<TPageObject>()
            where TPageObject : PageObject<TPageObject>
        {
            return GetPageObjectDefinition(typeof(TPageObject)).ComponentTypeName ?? "page object";
        }

        public static string ResolveControlTypeName<TControl>()
            where TControl : UIComponent
        {
            return ResolveControlTypeName(typeof(TControl));
        }

        public static string ResolveControlTypeName(Type type)
        {
            return GetControlDefinition(type).ComponentTypeName ?? NormalizeTypeName(type).ToString(TermFormat.Lower);
        }

        private static string NormalizeTypeName(Type type)
        {
            string typeName = type.Name;
            return typeName.Contains("`")
                ? typeName.Substring(0, typeName.IndexOf('`'))
                : typeName;
        }

        internal static ControlDefinitionAttribute GetControlDefinition(Type type)
        {
            return GetClassAttributes(type).OfType<ControlDefinitionAttribute>().FirstOrDefault() ?? new ControlDefinitionAttribute();
        }

        private static PageObjectDefinitionAttribute GetPageObjectDefinition(Type type)
        {
            return GetClassAttributes(type).OfType<PageObjectDefinitionAttribute>().FirstOrDefault() ?? new PageObjectDefinitionAttribute();
        }

        internal static Control<TOwner> GetControlByDelegate<TOwner>(Delegate controlDelegate)
            where TOwner : PageObject<TOwner>
        {
            controlDelegate.CheckNotNull("controlDelegate");

            UIComponent control;
            if (DelegateControls.TryGetValue(controlDelegate, out control))
                return (Control<TOwner>)control;
            else
                throw new ArgumentException("Failed to find mapped control by specified 'controlDelegate'.", "controlDelegate");
        }

        public static void CleanUpPageObjects(IEnumerable<UIComponent> pageObjects)
        {
            foreach (var item in pageObjects)
                CleanUpPageObject(item);
        }

        public static void CleanUpPageObject(UIComponent pageObject)
        {
            var delegatesToRemove = DelegateControls.Where(x => x.Value.Owner == pageObject).Select(x => x.Key).ToArray();
            foreach (var item in delegatesToRemove)
                DelegateControls.Remove(item);

            ClearComponentChildren(pageObject);
        }

        private static void ClearComponentChildren(UIComponent component)
        {
            foreach (var item in component.Children)
                ClearComponentChildren(item);

            component.Children.Clear();
        }
    }
}
