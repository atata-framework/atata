using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            RegisterDelegateControlMapping(typeof(ClickableDelegate<>), typeof(Clickable<>));
            RegisterDelegateControlMapping(typeof(ClickableDelegate<,>), typeof(Clickable<,>));

            RegisterDelegateControlMapping(typeof(LinkDelegate<>), typeof(Link<>));
            RegisterDelegateControlMapping(typeof(LinkDelegate<,>), typeof(Link<,>));

            RegisterDelegateControlMapping(typeof(ButtonDelegate<>), typeof(Button<>));
            RegisterDelegateControlMapping(typeof(ButtonDelegate<,>), typeof(Button<,>));
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

            TOwner componentAsPageObject = component as TOwner;
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

            UIComponentMetadata metadata = CreatePageObjectMetadata<TPageObject>();
            ApplyMetadata(pageObject, metadata);
        }

        // TODO: Review InitPageObjectTriggers method.
        private static void InitPageObjectTriggers<TOwner>(PageObject<TOwner> pageObject)
            where TOwner : PageObject<TOwner>
        {
            Type pageObjectType = pageObject.GetType();

            var classTriggers = GetClassAttributes(pageObjectType).OfType<TriggerAttribute>().Where(x => x.AppliesTo == TriggerScope.Self);
            foreach (TriggerAttribute trigger in classTriggers)
                trigger.IsDefinedAtComponentLevel = true;

            var assemblyTriggers = GetAssemblyAttributes(pageObjectType.Assembly).OfType<TriggerAttribute>();

            pageObject.Triggers = classTriggers.
                Concat(assemblyTriggers).
                OrderBy(x => x.Priority).
                ToArray();
        }

        private static IEnumerable<Type> GetAllInheritedTypes(Type type)
        {
            Type typeToCheck = type;
            while (typeToCheck != typeof(UIComponent) && (!typeToCheck.IsGenericType || (typeToCheck.GetGenericTypeDefinition() != typeof(UIComponent<>) && typeToCheck.GetGenericTypeDefinition() != typeof(PageObject<>))))
            {
                yield return typeToCheck;
                typeToCheck = typeToCheck.BaseType;
            }
        }

        // TODO: Refactor InitComponentTypeMembers method.
        private static void InitComponentTypeMembers<TOwner>(UIComponent<TOwner> component, Type type)
            where TOwner : PageObject<TOwner>
        {
            PropertyInfo[] suitableProperties = type.
                GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty | BindingFlags.SetProperty).
                Where(x => x.CanWrite && x.GetIndexParameters().Length == 0).
                Where(x => x.GetCustomAttribute<IgnoreInitAttribute>() == null).
                ToArray();

            Func<PropertyInfo, bool> isNullPropertyPredicate = x => x.GetValue(component) == null;

            PropertyInfo[] controlProperties = suitableProperties.
                Where(x => x.PropertyType.IsSubclassOfRawGeneric(typeof(Control<>))).
                Where(isNullPropertyPredicate).
                ToArray();

            foreach (var property in controlProperties)
                InitControlProperty<TOwner>(component, property);

            PropertyInfo[] componentPartProperties = suitableProperties.
                Where(x => x.PropertyType.IsSubclassOfRawGeneric(typeof(UIComponentPart<>))).
                Where(isNullPropertyPredicate).
                ToArray();

            foreach (var property in componentPartProperties)
                InitComponentPartProperty<TOwner>(component, property);

            PropertyInfo[] delegateProperties = suitableProperties.
                Where(x => typeof(MulticastDelegate).IsAssignableFrom(x.PropertyType.BaseType) && x.PropertyType.IsGenericType).
                Where(isNullPropertyPredicate).
                ToArray();

            foreach (var property in delegateProperties)
                InitDelegateProperty<TOwner>(component, property);
        }

        private static void InitControlProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            UIComponentMetadata metadata = CreateComponentMetadata<TOwner>(property);

            UIComponent<TOwner> component = CreateComponent(parentComponent, metadata);
            parentComponent.Controls.Add(component);

            property.SetValue(parentComponent, component, null);
        }

        private static void InitDelegateProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            Type controlType = ResolveDelegateControlType(property.PropertyType);

            if (controlType != null)
            {
                UIComponentMetadata metadata = CreateComponentMetadata<TOwner>(property, controlType);

                UIComponent<TOwner> component = CreateComponent(parentComponent, metadata);
                parentComponent.Controls.Add(component);

                Delegate clickDelegate = CreateDelegatePropertyDelegate(property, component);

                property.SetValue(parentComponent, clickDelegate, null);

                DelegateControls[clickDelegate] = component;
            }
        }

        private static Delegate CreateDelegatePropertyDelegate<TOwner>(PropertyInfo property, UIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            Type navigableInterfaceType = component.GetType().GetGenericInterfaceType(typeof(INavigable<,>));

            if (navigableInterfaceType != null)
            {
                var navigableGenericArguments = navigableInterfaceType.GetGenericArguments();

                var clickAndGoMethod = typeof(INavigableExtensions).
                    GetMethod(nameof(INavigableExtensions.ClickAndGo)).
                    MakeGenericMethod(navigableGenericArguments);

                return Delegate.CreateDelegate(property.PropertyType, component, clickAndGoMethod);
            }
            else
            {
                return Delegate.CreateDelegate(property.PropertyType, component, "Click");
            }
        }

        private static void InitComponentPartProperty<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property)
            where TOwner : PageObject<TOwner>
        {
            UIComponentPart<TOwner> componentPart = (UIComponentPart<TOwner>)ActivatorEx.CreateInstance(property.PropertyType);
            componentPart.Component = parentComponent;
            property.SetValue(parentComponent, componentPart, null);
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
            attributes = attributes?.Where(x => x != null).ToArray() ?? new Attribute[0];

            if (!attributes.OfType<NameAttribute>().Any())
            {
                attributes = attributes.Concat(new[]
                    {
                        new NameAttribute(name)
                    }).ToArray();
            }

            UIComponentMetadata metadata = CreateComponentMetadata<TOwner>(
                name,
                typeof(TComponent),
                parentComponent.GetType(),
                attributes,
                GetControlDefinition(typeof(TComponent)));

            var component = (TComponent)CreateComponent<TOwner>(parentComponent, metadata);

            parentComponent.Controls.Add(component);

            return component;
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
            findAttribute.Properties.Metadata = metadata;

            InitComponentLocator(component, metadata, findAttribute);
            component.ComponentName = ResolveControlName(metadata, findAttribute);
            component.ComponentTypeName = ResolveControlTypeName(component.GetType());
            component.CacheScopeElement = false;
            component.Triggers = GetControlTriggers(metadata);

            ApplyMetadata(component, metadata);
        }

        private static void ApplyMetadata<TOwner>(UIComponent<TOwner> component, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            foreach (IPropertySettings propertySettings in metadata.AllAttributes.OfType<IPropertySettings>().Where(x => x.Properties != null))
                propertySettings.Properties.Metadata = metadata;

            component.Metadata = metadata;
            component.ApplyMetadata(metadata);

            foreach (TriggerAttribute trigger in component.Triggers)
            {
                trigger.ApplyMetadata(metadata);
            }
        }

        private static void InitComponentLocator(UIComponent component, UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ComponentScopeLocateOptions locateOptions = CreateScopeLocateOptions(metadata, findAttribute);
            IComponentScopeLocateStrategy strategy = findAttribute.CreateStrategy(metadata);

            IItemsControl itemsControl = component as IItemsControl;

            component.ScopeSource = findAttribute.ScopeSource;

            if (itemsControl != null)
            {
                IFindItemAttribute findItemAttribute = GetPropertyFindItemAttribute(metadata);
                IItemElementFindStrategy itemElementFindStrategy = findItemAttribute.CreateStrategy(metadata);
                itemsControl.Apply(itemElementFindStrategy);
            }

            component.ScopeLocator = new StrategyScopeLocator(component, strategy, locateOptions);
        }

        private static string ResolveControlName(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            NameAttribute nameAttribute = metadata.GetFirstOrDefaultDeclaredAttribute<NameAttribute>();
            if (nameAttribute != null && !string.IsNullOrWhiteSpace(nameAttribute.Value))
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

            return metadata.ComponentDefinitonAttribute.
                NormalizeNameIgnoringEnding(metadata.Name).
                ToString(TermCase.Title);
        }

        private static UIComponentMetadata CreatePageObjectMetadata<TPageObject>()
        {
            Type type = typeof(TPageObject);

            // TODO: Review name set.
            return CreateComponentMetadata<TPageObject>(
                type.Name,
                type,
                null,
                new Attribute[0],
                GetPageObjectDefinition(type));
        }

        private static UIComponentMetadata CreateComponentMetadata<TOwner>(PropertyInfo property, Type propertyType = null)
        {
            return CreateComponentMetadata<TOwner>(
                property.Name,
                propertyType ?? property.PropertyType,
                property.DeclaringType,
                GetPropertyAttributes(property),
                GetControlDefinition(propertyType ?? property.PropertyType));
        }

        private static UIComponentMetadata CreateComponentMetadata<TOwner>(
            string name,
            Type componentType,
            Type parentComponentType,
            Attribute[] declaredAttributes,
            UIComponentDefinitionAttribute componentDefinitonAttribute)
        {
            return new UIComponentMetadata(
                name,
                componentType,
                parentComponentType,
                declaredAttributes,
                GetClassAttributes(componentType),
                GetClassAttributes(parentComponentType),
                GetAssemblyAttributes(typeof(TOwner).Assembly))
            {
                ComponentDefinitonAttribute = componentDefinitonAttribute
            };
        }

        private static FindAttribute GetPropertyFindAttribute(UIComponentMetadata metadata)
        {
            FindAttribute findAttribute = metadata.GetFirstOrDefaultDeclaredAttribute<FindAttribute>();
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
                    : GetDefaultFindAttribute(metadata);
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
                allFindingAttributes.Where(x => x.Depth == null && x.Attribute.ParentComponentType == null).Select(x => x.Attribute).FirstOrDefault();
        }

        private static FindAttribute GetDefaultFindAttribute(UIComponentMetadata metadata)
        {
            if (metadata.ComponentDefinitonAttribute.ScopeXPath == "*")
                return new UseParentScopeAttribute();

            return new FindFirstAttribute();
        }

        private static IFindItemAttribute GetPropertyFindItemAttribute(UIComponentMetadata metadata)
        {
            return metadata.GetFirstOrDefaultDeclaredAttribute<IFindItemAttribute>() ?? (IFindItemAttribute)new FindItemByLabelAttribute();
        }

        private static ComponentScopeLocateOptions CreateScopeLocateOptions(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ControlDefinitionAttribute definition = metadata.ComponentDefinitonAttribute as ControlDefinitionAttribute;

            int index = findAttribute.Index;

            ComponentScopeLocateOptions options = new ComponentScopeLocateOptions
            {
                Metadata = metadata,
                ElementXPath = definition != null ? definition.ScopeXPath : "*",
                Index = index >= 0 ? (int?)index : null,
                Visibility = findAttribute.Visibility
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

            foreach (TriggerAttribute trigger in resultTriggers)
                trigger.IsDefinedAtComponentLevel = true;

            List<TriggerAttribute> allOtherTriggers = metadata.DeclaredAttributes.
                Concat(metadata.ParentComponentAttributes.OfType<TriggerAttribute>().Where(x => x.AppliesTo == TriggerScope.Children)).
                Concat(metadata.AssemblyAttributes).
                OfType<TriggerAttribute>().
                ToList();

            while (allOtherTriggers.Count > 0)
            {
                TriggerAttribute currentTrigger = allOtherTriggers[0];
                Type currentTriggerType = currentTrigger.GetType();
                TriggerAttribute[] currentTriggersOfSameType = allOtherTriggers.Where(x => x.GetType() == currentTriggerType && x.On == currentTrigger.On).ToArray();

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

            return GetPageObjectDefinition(type).
                NormalizeNameIgnoringEnding(typeName).
                ToString(TermCase.Title);
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
            return GetControlDefinition(type).ComponentTypeName ?? NormalizeTypeName(type).ToString(TermCase.Lower);
        }

        public static string ResolveControlName<TControl, TOwner>(Expression<Func<TControl, bool>> predicateExpression)
            where TControl : Control<TOwner>
            where TOwner : PageObject<TOwner>
        {
            return ControlNameExpressionStringBuilder.ExpressionToString(predicateExpression);
        }

        private static string NormalizeTypeName(Type type)
        {
            string typeName = type.Name;
            return typeName.Contains("`")
                ? typeName.Substring(0, typeName.IndexOf('`'))
                : typeName;
        }

        public static ControlDefinitionAttribute GetControlDefinition(Type type)
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
            controlDelegate.CheckNotNull(nameof(controlDelegate));

            UIComponent control;
            if (DelegateControls.TryGetValue(controlDelegate, out control))
                return (Control<TOwner>)control;
            else
                throw new ArgumentException($"Failed to find mapped control by specified '{nameof(controlDelegate)}'.", nameof(controlDelegate));
        }

        public static Control<TOwner> GetChildControl<TOwner>(IUIComponent<TOwner> parent, string controlName)
            where TOwner : PageObject<TOwner>
        {
            PropertyInfo property = typeof(TOwner).GetPropertyWithThrowOnError(controlName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

            if (!property.PropertyType.IsSubclassOfRawGeneric(typeof(Control<>)))
                throw new InvalidOperationException($"Incorrect type of \"{controlName}\" property.");

            return (Control<TOwner>)property.GetValue(parent);
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

            pageObject.CleanUp();
        }
    }
}
