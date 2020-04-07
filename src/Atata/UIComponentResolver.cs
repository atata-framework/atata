using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Atata
{
    public static class UIComponentResolver
    {
        private static readonly ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> PropertyAttributes =
            new ConcurrentDictionary<ICustomAttributeProvider, Attribute[]>();

        private static readonly ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> ClassAttributes =
            new ConcurrentDictionary<ICustomAttributeProvider, Attribute[]>();

        private static readonly ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> AssemblyAttributes =
            new ConcurrentDictionary<ICustomAttributeProvider, Attribute[]>();

        private static readonly ConcurrentDictionary<Type, string> PageObjectNames =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, Type> DelegateControlsTypeMapping =
            new ConcurrentDictionary<Type, Type>
            {
                [typeof(ClickableDelegate<>)] = typeof(Clickable<>),
                [typeof(ClickableDelegate<,>)] = typeof(Clickable<,>),

                [typeof(LinkDelegate<>)] = typeof(Link<>),
                [typeof(LinkDelegate<,>)] = typeof(Link<,>),

                [typeof(ButtonDelegate<>)] = typeof(Button<>),
                [typeof(ButtonDelegate<,>)] = typeof(Button<,>)
            };

        private static readonly ConcurrentDictionary<Delegate, UIComponent> DelegateControls =
            new ConcurrentDictionary<Delegate, UIComponent>();

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
                metadata.Push(attributes);

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

            InitComponentLocator(component, metadata);
            component.ComponentName = ResolveControlName(metadata);
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

        private static void InitComponentLocator(UIComponent component, UIComponentMetadata metadata)
        {
            // TODO: Remove this condition when IItemsControl will be removed.
#pragma warning disable CS0618
            if (component is IItemsControl itemsControl)
#pragma warning restore CS0618
            {
                IFindItemAttribute findItemAttribute = GetPropertyFindItemAttribute(metadata);
                IItemElementFindStrategy itemElementFindStrategy = findItemAttribute.CreateStrategy(component, metadata);
                itemsControl.Apply(itemElementFindStrategy);
            }

            component.ScopeLocator = new StrategyScopeLocator(
                new StrategyScopeLocatorExecutionDataCollector(component),
                StrategyScopeLocatorExecutor.Default);
        }

        private static string ResolveControlName(UIComponentMetadata metadata)
        {
            return GetControlNameFromNameAttribute(metadata)
                ?? GetControlNameFromFindAttribute(metadata)
                ?? GetComponentNameFromMetadata(metadata);
        }

        private static string GetControlNameFromNameAttribute(UIComponentMetadata metadata)
        {
            NameAttribute nameAttribute = metadata.Get<NameAttribute>(x => x.At(AttributeLevels.Declared));

            return !string.IsNullOrWhiteSpace(nameAttribute?.Value)
                ? nameAttribute.Value
                : null;
        }

        private static string GetControlNameFromFindAttribute(UIComponentMetadata metadata)
        {
            FindAttribute findAttribute = metadata.ResolveFindAttribute();

            if (findAttribute is FindByLabelAttribute findByLabelAttribute && findByLabelAttribute.Match == TermMatch.Equals)
            {
                string[] terms = findByLabelAttribute.Values;

                if (terms?.Any() ?? false)
                {
                    return string.Join("/", terms);
                }
            }

            return null;
        }

        private static string GetComponentNameFromMetadata(UIComponentMetadata metadata)
        {
            return metadata.ComponentDefinitionAttribute.
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
            return new UIComponentMetadata(name, componentType, parentComponentType)
            {
                DeclaredAttributesList = declaredAttributes.ToList(),
                ParentComponentAttributesList = GetClassAttributes(parentComponentType).ToList(),
                AssemblyAttributesList = GetAssemblyAttributes(typeof(TOwner).Assembly).ToList(),
                GlobalAttributesList = new List<Attribute>(),
                ComponentAttributesList = GetClassAttributes(componentType).ToList()
            };
        }

        // TODO: Remove this method when IItemsControl will be removed.
        private static IFindItemAttribute GetPropertyFindItemAttribute(UIComponentMetadata metadata)
        {
            return metadata.Get<IFindItemAttribute>(x => x.At(AttributeLevels.Declared)) ?? new FindItemByLabelAttribute();
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

        private static Attribute[] ResolveAndCacheAttributes(ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> cache, ICustomAttributeProvider attributeProvider)
        {
            if (attributeProvider == null)
                return new Attribute[0];

            return cache.GetOrAdd(attributeProvider, x => x.GetCustomAttributes(true).Cast<Attribute>().ToArray());
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
            return PageObjectNames.GetOrAdd(typeof(TPageObject), ResolvePageObjectNameFromMetadata);
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
            return ObjectExpressionStringBuilder.ExpressionToString(predicateExpression);
        }

        private static string NormalizeTypeName(Type type)
        {
            string typeName = type.Name;
            return typeName.Contains("`")
                ? typeName.Substring(0, typeName.IndexOf('`'))
                : typeName;
        }

        public static string ResolveComponentFullName<TOwner>(object component)
            where TOwner : PageObject<TOwner>
        {
            return component is IUIComponent<TOwner> uiComponent
                ? uiComponent.ComponentFullName
                : component is UIComponentPart<TOwner> uiComponentPart
                ? $"{uiComponentPart.Component.ComponentFullName} {uiComponentPart.ComponentPartName}"
                : component is IDataProvider<object, TOwner> dataProvider
                ? $"{dataProvider.Component.ComponentFullName} {dataProvider.ProviderName}"
                : null;
        }

        public static ControlDefinitionAttribute GetControlDefinition(Type type)
        {
            return GetClassAttributes(type).OfType<ControlDefinitionAttribute>().FirstOrDefault() ?? new ControlDefinitionAttribute();
        }

        public static ControlDefinitionAttribute GetControlDefinition(UIComponentMetadata metadata)
        {
            return (metadata.ComponentDefinitionAttribute as ControlDefinitionAttribute) ?? new ControlDefinitionAttribute();
        }

        public static PageObjectDefinitionAttribute GetPageObjectDefinition(Type type)
        {
            return GetClassAttributes(type).OfType<PageObjectDefinitionAttribute>().FirstOrDefault() ?? new PageObjectDefinitionAttribute();
        }

        public static PageObjectDefinitionAttribute GetPageObjectDefinition(UIComponentMetadata metadata)
        {
            return (metadata.ComponentDefinitionAttribute as PageObjectDefinitionAttribute) ?? new PageObjectDefinitionAttribute();
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
                DelegateControls.TryRemove(item, out var removed);

            pageObject.CleanUp();
        }
    }
}
