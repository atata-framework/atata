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
        private static readonly ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> s_propertyAttributes =
            new ConcurrentDictionary<ICustomAttributeProvider, Attribute[]>();

        private static readonly ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> s_classAttributes =
            new ConcurrentDictionary<ICustomAttributeProvider, Attribute[]>();

        private static readonly ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> s_assemblyAttributes =
            new ConcurrentDictionary<ICustomAttributeProvider, Attribute[]>();

        private static readonly ConcurrentDictionary<Type, string> s_pageObjectNames =
            new ConcurrentDictionary<Type, string>();

        private static readonly ConcurrentDictionary<Type, Type> s_delegateControlsTypeMapping =
            new ConcurrentDictionary<Type, Type>
            {
                [typeof(ClickableDelegate<>)] = typeof(Clickable<>),
                [typeof(ClickableDelegate<,>)] = typeof(Clickable<,>),

                [typeof(LinkDelegate<>)] = typeof(Link<>),
                [typeof(LinkDelegate<,>)] = typeof(Link<,>),

                [typeof(ButtonDelegate<>)] = typeof(Button<>),
                [typeof(ButtonDelegate<,>)] = typeof(Button<,>)
            };

        private static readonly ConcurrentDictionary<Delegate, UIComponent> s_delegateControls =
            new ConcurrentDictionary<Delegate, UIComponent>();

        public static void RegisterDelegateControlMapping(Type delegateType, Type controlType)
        {
            s_delegateControlsTypeMapping[delegateType] = controlType;
        }

        public static void Resolve<TOwner>(UIComponent<TOwner> component)
            where TOwner : PageObject<TOwner>
        {
            Type[] allTypes = GetAllInheritedTypes(component.GetType()).Reverse().ToArray();

            foreach (Type type in allTypes)
                InitComponentTypeMembers(component, type);
        }

        internal static void InitPageObject<TPageObject>(PageObject<TPageObject> pageObject)
            where TPageObject : PageObject<TPageObject>
        {
            pageObject.Owner = (TPageObject)pageObject;

            UIComponentMetadata metadata = CreatePageObjectMetadata<TPageObject>();

            ApplyMetadata(pageObject, metadata);
        }

        private static IEnumerable<Type> GetAllInheritedTypes(Type type)
        {
            Type typeToCheck = type;

            while (
                typeToCheck != typeof(UIComponent) &&
                (!typeToCheck.IsGenericType || (typeToCheck.GetGenericTypeDefinition() != typeof(UIComponent<>) && typeToCheck.GetGenericTypeDefinition() != typeof(PageObject<>))))
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

            bool IsNullPropertyPredicate(PropertyInfo property) =>
                property.GetValue(component) == null;

            PropertyInfo[] controlProperties = suitableProperties.
                Where(x => x.PropertyType.IsSubclassOfRawGeneric(typeof(Control<>))).
                Where(IsNullPropertyPredicate).
                ToArray();

            foreach (var property in controlProperties)
                InitControlProperty(component, property);

            PropertyInfo[] componentPartProperties = suitableProperties.
                Where(x => x.PropertyType.IsSubclassOfRawGeneric(typeof(UIComponentPart<>))).
                Where(IsNullPropertyPredicate).
                ToArray();

            foreach (var property in componentPartProperties)
                InitComponentPartProperty(component, property);

            PropertyInfo[] delegateProperties = suitableProperties.
                Where(x => typeof(MulticastDelegate).IsAssignableFrom(x.PropertyType.BaseType) && x.PropertyType.IsGenericType).
                Where(IsNullPropertyPredicate).
                ToArray();

            foreach (var property in delegateProperties)
                InitDelegateProperty(component, property);
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

                s_delegateControls[clickDelegate] = component;
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
            {
                supportsMetadata.Metadata = CreateStaticControlMetadata(parentComponent, property, supportsMetadata.ComponentType);

                string nameFromMetadata = GetControlNameFromNameAttribute(supportsMetadata.Metadata);

                if (nameFromMetadata != null)
                    componentPart.ComponentPartName = nameFromMetadata;
            }

            property.SetValue(parentComponent, componentPart, null);
        }

        private static Type ResolveDelegateControlType(Type delegateType)
        {
            Type delegateGenericTypeDefinition = delegateType.GetGenericTypeDefinition();

            if (s_delegateControlsTypeMapping.TryGetValue(delegateGenericTypeDefinition, out Type controlGenericTypeDefinition))
            {
                Type[] genericArguments = delegateType.GetGenericArguments();
                return controlGenericTypeDefinition.MakeGenericType(genericArguments);
            }

            return null;
        }

        public static TComponentPart CreateComponentPart<TComponentPart, TOwner>(UIComponent<TOwner> parentComponent, string name, params Attribute[] attributes)
            where TComponentPart : UIComponentPart<TOwner>
            where TOwner : PageObject<TOwner>
        {
            parentComponent.CheckNotNull(nameof(parentComponent));
            name.CheckNotNull(nameof(name));

            TComponentPart componentPart = ActivatorEx.CreateInstance<TComponentPart>();
            componentPart.Component = parentComponent;
            componentPart.ComponentPartName = name;

            attributes = attributes?.Where(x => x != null).ToArray() ?? new Attribute[0];

            if (componentPart is ISupportsMetadata supportsMetadata)
            {
                supportsMetadata.Metadata = CreateComponentMetadata(
                    parentComponent,
                    name,
                    supportsMetadata.ComponentType,
                    attributes);

                string nameFromMetadata = GetControlNameFromNameAttribute(supportsMetadata.Metadata);

                if (nameFromMetadata != null)
                    componentPart.ComponentPartName = nameFromMetadata;
            }

            return componentPart;
        }

        public static TComponent CreateControl<TComponent, TOwner>(UIComponent<TOwner> parentComponent, string name, params Attribute[] attributes)
            where TComponent : UIComponent<TOwner>
            where TOwner : PageObject<TOwner>
        {
            parentComponent.CheckNotNull(nameof(parentComponent));

            attributes = attributes?.Where(x => x != null).ToArray() ?? new Attribute[0];

            if (!attributes.OfType<NameAttribute>().Any())
            {
                attributes = attributes.Concat(new[]
                    {
                        new NameAttribute(name)
                    }).ToArray();
            }

            UIComponentMetadata metadata = CreateComponentMetadata(
                parentComponent,
                name,
                typeof(TComponent),
                attributes);

            return (TComponent)CreateComponent(parentComponent, metadata);
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

            ApplyMetadata(component, metadata);
        }

        private static void ApplyMetadata<TOwner>(UIComponent<TOwner> component, UIComponentMetadata metadata)
            where TOwner : PageObject<TOwner>
        {
            foreach (IPropertySettings propertySettings in metadata.AllAttributes.OfType<IPropertySettings>().Where(x => x.Properties != null))
                propertySettings.Properties.Metadata = metadata;

            component.Metadata = metadata;
            component.ApplyMetadata(metadata);
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
            NameAttribute nameAttribute = metadata.Get<NameAttribute>();

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
            if (metadata.Name is null)
            {
                FindAttribute findAttribute = metadata.ResolveFindAttribute();

                return findAttribute.BuildComponentName();
            }
            else
            {
                return metadata.ComponentDefinitionAttribute.
                    NormalizeNameIgnoringEnding(metadata.Name).
                    ToString(TermCase.Title);
            }
        }

        private static UIComponentMetadata CreatePageObjectMetadata<TPageObject>()
            where TPageObject : PageObject<TPageObject>
        {
            Type type = typeof(TPageObject);

            // TODO: Review name set.
            return CreateComponentMetadata<TPageObject>(
                null,
                type.Name,
                type,
                new Attribute[0]);
        }

        private static UIComponentMetadata CreateStaticControlMetadata<TOwner>(
            UIComponent<TOwner> parentComponent,
            PropertyInfo property,
            Type propertyType = null)
            where TOwner : PageObject<TOwner>
        {
            return CreateComponentMetadata(
                parentComponent,
                property.Name,
                propertyType ?? property.PropertyType,
                GetPropertyAttributes(property));
        }

        private static UIComponentMetadata CreateComponentMetadata<TOwner>(
            UIComponent<TOwner> parentComponent,
            string name,
            Type componentType,
            Attribute[] declaredAttributes)
            where TOwner : PageObject<TOwner>
        {
            Type parentComponentType = parentComponent?.GetType();

            AtataAttributesContext contextAttributes = AtataContext.Current.Attributes;
            UIComponentMetadata metadata = new UIComponentMetadata(name, componentType, parentComponentType);

            // Declared:
            metadata.DeclaredAttributesList.AddRange(declaredAttributes);

            if (parentComponent != null)
            {
                var propertyContextAttributes = contextAttributes.PropertyMap
                   .Where(x => x.Key.PropertyName == name)
                   .Select(pair => new { Depth = parentComponentType.GetDepthOfInheritance(pair.Key.Type), Attributes = pair.Value })
                   .Where(x => x.Depth != null)
                   .OrderBy(x => x.Depth)
                   .SelectMany(x => x.Attributes.AsEnumerable().Reverse());

                metadata.DeclaredAttributesList.InsertRange(0, propertyContextAttributes);

                // Parent:
                metadata.ParentDeclaredAttributesList = parentComponent.Metadata.DeclaredAttributesList;
                metadata.ParentComponentAttributesList = parentComponent.Metadata.ComponentAttributesList;
            }

            // Assembly:
            Assembly ownerAssembly = typeof(TOwner).Assembly;

            if (contextAttributes.AssemblyMap.TryGetValue(ownerAssembly, out var contextAssemblyAttributes))
                metadata.AssemblyAttributesList.AddRange(contextAssemblyAttributes.AsEnumerable().Reverse());

            metadata.AssemblyAttributesList.AddRange(GetAssemblyAttributes(ownerAssembly));

            // Global:
            metadata.GlobalAttributesList.AddRange(contextAttributes.Global.AsEnumerable().Reverse());

            // Component:
            var componentContextAttributes = contextAttributes.ComponentMap
               .Select(pair => new { Depth = componentType.GetDepthOfInheritance(pair.Key), Attributes = pair.Value })
               .Where(x => x.Depth != null)
               .OrderBy(x => x.Depth)
               .SelectMany(x => x.Attributes.AsEnumerable().Reverse());

            metadata.ComponentAttributesList.AddRange(componentContextAttributes);
            metadata.ComponentAttributesList.AddRange(GetClassAttributes(componentType));

            return metadata;
        }

        // TODO: Remove this method when IItemsControl will be removed.
        private static IFindItemAttribute GetPropertyFindItemAttribute(UIComponentMetadata metadata)
        {
            return metadata.Get<IFindItemAttribute>(x => x.At(AttributeLevels.Declared)) ?? new FindItemByLabelAttribute();
        }

        private static Attribute[] ResolveAndCacheAttributes(ConcurrentDictionary<ICustomAttributeProvider, Attribute[]> cache, ICustomAttributeProvider attributeProvider)
        {
            if (attributeProvider == null)
                return new Attribute[0];

            return cache.GetOrAdd(attributeProvider, x => x.GetCustomAttributes(true).Cast<Attribute>().ToArray());
        }

        private static Attribute[] GetPropertyAttributes(PropertyInfo property)
        {
            return ResolveAndCacheAttributes(s_propertyAttributes, property);
        }

        private static Attribute[] GetClassAttributes(Type type)
        {
            return ResolveAndCacheAttributes(s_classAttributes, type);
        }

        private static Attribute[] GetAssemblyAttributes(Assembly assembly)
        {
            return ResolveAndCacheAttributes(s_assemblyAttributes, assembly);
        }

        public static string ResolvePageObjectName<TPageObject>()
            where TPageObject : PageObject<TPageObject>
        {
            return s_pageObjectNames.GetOrAdd(typeof(TPageObject), ResolvePageObjectNameFromMetadata);
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

            if (s_delegateControls.TryGetValue(controlDelegate, out UIComponent control))
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
            var delegatesToRemove = s_delegateControls.Where(x => x.Value.Owner == pageObject).Select(x => x.Key).ToArray();

            foreach (var item in delegatesToRemove)
                s_delegateControls.TryRemove(item, out var removed);

            pageObject.CleanUp();
        }
    }
}
