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

            if (component is TOwner componentAsPageObject)
                InitPageObject(componentAsPageObject);
        }

        private static void InitPageObject<TPageObject>(PageObject<TPageObject> pageObject)
            where TPageObject : PageObject<TPageObject>
        {
            pageObject.Owner = (TPageObject)pageObject;

            UIComponentMetadata metadata = CreatePageObjectMetadata<TPageObject>();
            InitPageObjectTriggers(pageObject, metadata);

            ApplyMetadata(pageObject, metadata);
        }

        private static void InitPageObjectTriggers<TOwner>(PageObject<TOwner> pageObject, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            var componentTriggers = metadata.ComponentAttributes.
                OfType<TriggerAttribute>().
                Where(x => x.AppliesTo == TriggerScope.Self);

            pageObject.Triggers.ComponentTriggersList.AddRange(componentTriggers);

            var assemblyTriggers = metadata.AssemblyAttributes.
                OfType<TriggerAttribute>();

            pageObject.Triggers.AssemblyTriggersList.AddRange(assemblyTriggers);

            pageObject.Triggers.Reorder();
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
            UIComponentMetadata metadata = CreateStaticControlMetadata(parentComponent, property);

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
                UIComponentMetadata metadata = CreateStaticControlMetadata(parentComponent, property, controlType);

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
            componentPart.ComponentPartName = property.Name.ToString(TermCase.MidSentence);

            if (componentPart is ISupportsMetadata supportsMetadata)
                supportsMetadata.Metadata = CreateStaticControlMetadata(parentComponent, property, supportsMetadata.ComponentType);

            property.SetValue(parentComponent, componentPart, null);
        }

        private static Type ResolveDelegateControlType(Type delegateType)
        {
            Type delegateGenericTypeDefinition = delegateType.GetGenericTypeDefinition();

            if (DelegateControlsTypeMapping.TryGetValue(delegateGenericTypeDefinition, out Type controlGenericTypeDefinition))
            {
                Type[] genericArguments = delegateType.GetGenericArguments();
                return controlGenericTypeDefinition.MakeGenericType(genericArguments);
            }

            return null;
        }

        public static TComponent CreateControl<TComponent, TOwner>(UIComponent<TOwner> parentComponent, string name, params Attribute[] attributes)
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
                attributes);

            var component = (TComponent)CreateComponent(parentComponent, metadata);

            parentComponent.Controls.Add(component);

            return component;
        }

        public static TComponent CreateControlForProperty<TComponent, TOwner>(UIComponent<TOwner> parentComponent, string propertyName, params Attribute[] attributes)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            PropertyInfo property = parentComponent.GetType().GetPropertyWithThrowOnError(propertyName, typeof(TComponent), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            UIComponentMetadata metadata = CreateStaticControlMetadata(parentComponent, property);

            if (attributes != null)
                metadata.DeclaredAttributesList.AddRange(attributes);

            var component = (TComponent)CreateComponent(parentComponent, metadata);

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

            FindAttribute findAttribute = GetPropertyFindAttribute(metadata);
            findAttribute.Properties.Metadata = metadata;

            InitComponentLocator(component, metadata, findAttribute);
            component.ComponentName = ResolveControlName(metadata, findAttribute);
            component.ComponentTypeName = ResolveControlTypeName(metadata);
            component.CacheScopeElement = false;
            InitControlTriggers(component, metadata);

            ApplyMetadata(component, metadata);
        }

        private static void ApplyMetadata<TOwner>(UIComponent<TOwner> component, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            foreach (IPropertySettings propertySettings in metadata.AllAttributes.OfType<IPropertySettings>().Where(x => x.Properties != null))
                propertySettings.Properties.Metadata = metadata;

            component.Metadata = metadata;
            component.ApplyMetadata(metadata);
            component.Triggers.ApplyMetadata(metadata);
        }

        private static void InitComponentLocator(UIComponent component, UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ComponentScopeLocateOptions locateOptions = CreateScopeLocateOptions(metadata, findAttribute);
            IComponentScopeLocateStrategy strategy = findAttribute.CreateStrategy(metadata);

            component.ScopeSource = findAttribute.ScopeSource;

            if (component is IItemsControl itemsControl)
            {
                IFindItemAttribute findItemAttribute = GetPropertyFindItemAttribute(metadata);
                IItemElementFindStrategy itemElementFindStrategy = findItemAttribute.CreateStrategy(component, metadata);
                itemsControl.Apply(itemElementFindStrategy);
            }

            component.ScopeLocator = new StrategyScopeLocator(component, strategy, locateOptions);
        }

        private static string ResolveControlName(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            NameAttribute nameAttribute = metadata.Get<NameAttribute>(AttributeLevels.Declared);

            if (!string.IsNullOrWhiteSpace(nameAttribute?.Value))
            {
                return nameAttribute.Value;
            }

            if (findAttribute is FindByLabelAttribute findByLabelAttribute && findByLabelAttribute.Match == TermMatch.Equals)
            {
                if (findByLabelAttribute.Values?.Any() ?? false)
                {
                    return string.Join("/", findByLabelAttribute.Values);
                }
                else
                {
                    TermAttribute termAttribute = metadata.Get<TermAttribute>(AttributeLevels.Declared);
                    if (termAttribute?.Values?.Any() ?? false)
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
                new Attribute[0]);
        }

        private static UIComponentMetadata CreateStaticControlMetadata<TOwner>(UIComponent<TOwner> parentComponent, PropertyInfo property, Type propertyType = null)
            where TOwner : PageObject<TOwner>
        {
            return CreateComponentMetadata<TOwner>(
                property.Name,
                propertyType ?? property.PropertyType,
                parentComponent.GetType(),
                GetPropertyAttributes(property));
        }

        private static UIComponentMetadata CreateComponentMetadata<TOwner>(
            string name,
            Type componentType,
            Type parentComponentType,
            Attribute[] declaredAttributes)
        {
            UIComponentMetadata metadata = new UIComponentMetadata(name, componentType, parentComponentType)
            {
                DeclaredAttributesList = declaredAttributes.ToList(),
                ParentComponentAttributesList = GetClassAttributes(parentComponentType).ToList(),
                AssemblyAttributesList = GetAssemblyAttributes(typeof(TOwner).Assembly).ToList(),
                GlobalAttributesList = new List<Attribute>(),
                ComponentAttributesList = GetClassAttributes(componentType).ToList()
            };

            if (parentComponentType == null)
                metadata.ComponentDefinitonAttribute = GetPageObjectDefinition(metadata);
            else
                metadata.ComponentDefinitonAttribute = GetControlDefinition(metadata);

            return metadata;
        }

        private static FindAttribute GetPropertyFindAttribute(UIComponentMetadata metadata)
        {
            FindAttribute findAttribute = metadata.Get<FindAttribute>(AttributeLevels.Declared);
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
                Select(attr => new { Attribute = attr, Depth = controlType.GetDepthOfInheritance(attr.ControlType) }).
                Where(x => x.Depth != null).
                OrderBy(x => x.Depth).
                Select(x => x.Attribute).
                FirstOrDefault(attr => attr.ParentComponentType == null || parentComponentType.IsInheritedFromOrIs(attr.ParentComponentType));
        }

        private static ControlFindingAttribute GetNearestDefaultControlFindingAttribute(Type parentComponentType, IEnumerable<Attribute> attributes)
        {
            var allFindingAttributes = attributes.OfType<ControlFindingAttribute>().
                Where(x => x.ControlType == null).
                Select(attr => new { Attribute = attr, Depth = parentComponentType.GetDepthOfInheritance(attr.ParentComponentType) }).
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
            return metadata.Get<IFindItemAttribute>(AttributeLevels.Declared) ?? new FindItemByLabelAttribute();
        }

        private static ComponentScopeLocateOptions CreateScopeLocateOptions(UIComponentMetadata metadata, FindAttribute findAttribute)
        {
            ControlDefinitionAttribute definition = metadata.ComponentDefinitonAttribute as ControlDefinitionAttribute;

            int index = findAttribute.Index;

            ComponentScopeLocateOptions options = new ComponentScopeLocateOptions
            {
                Metadata = metadata,
                ElementXPath = definition?.ScopeXPath ?? ScopeDefinitionAttribute.DefaultScopeXPath,
                Index = index >= 0 ? (int?)index : null,
                Visibility = findAttribute.Visibility,
                OuterXPath = findAttribute.OuterXPath
            };

            ITermFindAttribute termFindAttribute = findAttribute as ITermFindAttribute;
            ITermMatchFindAttribute termMatchFindAttribute = findAttribute as ITermMatchFindAttribute;

            if (termFindAttribute != null)
                options.Terms = termFindAttribute.GetTerms(metadata);

            if (termMatchFindAttribute != null)
                options.Match = termMatchFindAttribute.GetTermMatch(metadata);

            return options;
        }

        private static void InitControlTriggers<TOwner>(UIComponent<TOwner> component, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            var componentTriggers = metadata.ComponentAttributes.
                OfType<TriggerAttribute>().
                Where(x => x.AppliesTo == TriggerScope.Self);

            component.Triggers.ComponentTriggersList.AddRange(componentTriggers);

            var parentComponentTriggers = metadata.ParentComponentAttributes.
                OfType<TriggerAttribute>().
                Where(x => x.AppliesTo == TriggerScope.Children);

            component.Triggers.ParentComponentTriggersList.AddRange(parentComponentTriggers);

            var assemblyTriggers = metadata.AssemblyAttributes.
                OfType<TriggerAttribute>();

            component.Triggers.AssemblyTriggersList.AddRange(assemblyTriggers);

            var declaredTriggers = metadata.DeclaredAttributes.
                OfType<TriggerAttribute>().
                Where(x => x.AppliesTo == TriggerScope.Self);

            component.Triggers.DeclaredTriggersList.AddRange(declaredTriggers);

            component.Triggers.Reorder();
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

            if (PageObjectNames.TryGetValue(type, out string name))
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
            ControlDefinitionAttribute controlDefinitionAttribute = GetControlDefinition(type);
            return ResolveControlTypeName(controlDefinitionAttribute, type);
        }

        public static string ResolveControlTypeName(UIComponentMetadata metadata)
        {
            ControlDefinitionAttribute controlDefinitionAttribute = GetControlDefinition(metadata);
            return ResolveControlTypeName(controlDefinitionAttribute, metadata.ComponentType);
        }

        public static string ResolveControlTypeName(ControlDefinitionAttribute controlDefinitionAttribute, Type controlType)
        {
            return controlDefinitionAttribute.ComponentTypeName ?? NormalizeTypeName(controlType).ToString(TermCase.MidSentence);
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

        public static ControlDefinitionAttribute GetControlDefinition(UIComponentMetadata metadata)
        {
            return metadata.Get<ControlDefinitionAttribute>(AttributeLevels.Declared | AttributeLevels.Component) ?? new ControlDefinitionAttribute();
        }

        public static PageObjectDefinitionAttribute GetPageObjectDefinition(Type type)
        {
            return GetClassAttributes(type).OfType<PageObjectDefinitionAttribute>().FirstOrDefault() ?? new PageObjectDefinitionAttribute();
        }

        public static PageObjectDefinitionAttribute GetPageObjectDefinition(UIComponentMetadata metadata)
        {
            return metadata.Get<PageObjectDefinitionAttribute>(AttributeLevels.Declared | AttributeLevels.Component) ?? new PageObjectDefinitionAttribute();
        }

        internal static Control<TOwner> GetControlByDelegate<TOwner>(Delegate controlDelegate)
            where TOwner : PageObject<TOwner>
        {
            controlDelegate.CheckNotNull(nameof(controlDelegate));

            if (DelegateControls.TryGetValue(controlDelegate, out UIComponent control))
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
