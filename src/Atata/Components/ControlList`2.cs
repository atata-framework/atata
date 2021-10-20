using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the list of controls of <typeparamref name="TItem"/> type.
    /// </summary>
    /// <typeparam name="TItem">The type of the item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class ControlList<TItem, TOwner> :
        UIComponentPart<TOwner>,
        ISupportsMetadata,
        IDataProvider<IEnumerable<TItem>, TOwner>,
        IEnumerable<TItem>,
        IClearsCache
        where TItem : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected const string GetElementValuesScript = @"
var elements = arguments[0];
var textValues = [];

for (var i = 0; i < elements.length; i++) {
    textValues.push(elements[i].{0});
}

return textValues;";

        private readonly Dictionary<string, TItem> _cachedNamedItemsMap = new Dictionary<string, TItem>();

        private readonly Dictionary<(Visibility Visibility, string ExtraXPath), ReadOnlyCollection<IWebElement>> _cachedAllElementsMap =
            new Dictionary<(Visibility, string ExtraXPath), ReadOnlyCollection<IWebElement>>();

        private readonly Dictionary<IWebElement, TItem> _cachedElementItemsMap = new Dictionary<IWebElement, TItem>();

        private string _itemComponentTypeName;

        [Obsolete("This property is not used internally anymore, no sense to use it.")] // Obsolete since v1.5.0.
        protected ControlDefinitionAttribute ItemDefinition =>
            (ControlDefinitionAttribute)Metadata.ComponentDefinitionAttribute;

        [Obsolete("This property is not used internally anymore, no sense to use it.")] // Obsolete since v1.5.0.
        protected FindAttribute ItemFindAttribute =>
            ResolveItemFindAttribute();

        protected string ItemComponentTypeName =>
            _itemComponentTypeName ?? (_itemComponentTypeName = UIComponentResolver.ResolveControlTypeName(Metadata));

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public DataVerificationProvider<IEnumerable<TItem>, TOwner> Should => new DataVerificationProvider<IEnumerable<TItem>, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public DataVerificationProvider<IEnumerable<TItem>, TOwner> ExpectTo => Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public DataVerificationProvider<IEnumerable<TItem>, TOwner> WaitTo => Should.Using<WaitingVerificationStrategy>();

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the controls count.
        /// </summary>
        public DataProvider<int, TOwner> Count => Component.GetOrCreateDataProvider($"{ComponentPartName} count", GetCount);

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the controls contents.
        /// </summary>
        public DataProvider<IEnumerable<string>, TOwner> Contents => Component.GetOrCreateDataProvider($"{ComponentPartName} contents", GetContents);

        UIComponent IDataProvider<IEnumerable<TItem>, TOwner>.Component => (UIComponent)Component;

        protected string ProviderName => $"{ComponentPartName}";

        string IObjectProvider<IEnumerable<TItem>>.ProviderName => ProviderName;

        TOwner IDataProvider<IEnumerable<TItem>, TOwner>.Owner => Component.Owner;

        TermOptions IDataProvider<IEnumerable<TItem>, TOwner>.ValueTermOptions { get; }

        IEnumerable<TItem> IObjectProvider<IEnumerable<TItem>>.Value => GetAll();

        UIComponentMetadata ISupportsMetadata.Metadata
        {
            get { return Metadata; }
            set { Metadata = value; }
        }

        public UIComponentMetadata Metadata { get; private set; }

        Type ISupportsMetadata.ComponentType
        {
            get { return typeof(TItem); }
        }

        protected bool UseScopeCache =>
            Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute || x is UsesScopeCacheAttribute))
                ?.UseCache ?? false;

        /// <summary>
        /// Gets the control at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to get.</param>
        /// <returns>The item at the specified index.</returns>
        public TItem this[int index]
        {
            get
            {
                index.CheckIndexNonNegative();

                return GetItemByIndex(index);
            }
        }

        /// <summary>
        /// Gets the control that matches the conditions defined by the specified predicate expression.
        /// </summary>
        /// <param name="predicateExpression">The predicate expression to test each item.</param>
        /// <returns>The first item that matches the conditions of the specified predicate.</returns>
        public TItem this[Expression<Func<TItem, bool>> predicateExpression]
        {
            get
            {
                predicateExpression.CheckNotNull(nameof(predicateExpression));

                string itemName = UIComponentResolver.ResolveControlName<TItem, TOwner>(predicateExpression);

                return GetItem(itemName, predicateExpression);
            }
        }

        /// <summary>
        /// Gets the control that matches the specified XPath condition.
        /// </summary>
        /// <param name="xPathCondition">
        /// The XPath condition.
        /// For example: <c>"@some-attr='some value'"</c>.</param>
        /// <returns>The first item that matches the XPath condition.</returns>
        public TItem GetByXPathCondition(string xPathCondition) =>
            GetByXPathCondition(null, xPathCondition);

        /// <summary>
        /// Gets the control that matches the specified XPath condition.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="xPathCondition">
        /// The XPath condition.
        /// For example: <c>"@some-attr='some value'"</c>.</param>
        /// <returns>The first item that matches the XPath condition.</returns>
        public TItem GetByXPathCondition(string itemName, string xPathCondition)
        {
            xPathCondition.CheckNotNullOrEmpty(nameof(xPathCondition));

            itemName = itemName ?? $"XPath: '{xPathCondition}'";

            return GetItemByInnerXPath(itemName, xPathCondition);
        }

        /// <summary>
        /// Gets all controls of this list that match the specified XPath condition.
        /// </summary>
        /// <param name="xPathCondition">
        /// The XPath condition.
        /// For example: <c>"@some-attr='some value'"</c>.</param>
        /// <returns>All items that match the XPath condition.</returns>
        public DataProvider<IEnumerable<TItem>, TOwner> GetAllByXPathCondition(string xPathCondition) =>
            GetAllByXPathCondition(null, xPathCondition);

        /// <summary>
        /// Gets all controls of this list that match the specified XPath condition.
        /// </summary>
        /// <param name="itemsName">Name of the items to use in reporting.</param>
        /// <param name="xPathCondition">
        /// The XPath condition.
        /// For example: <c>"@some-attr='some value'"</c>.</param>
        /// <returns>All items that match the XPath condition.</returns>
        public DataProvider<IEnumerable<TItem>, TOwner> GetAllByXPathCondition(string itemsName, string xPathCondition)
        {
            xPathCondition.CheckNotNullOrEmpty(nameof(xPathCondition));

            string extraXPath = xPathCondition[0] == '['
                ? xPathCondition
                : $"[{xPathCondition}]";

            itemsName = itemsName ?? $"XPath: '{extraXPath}'";

            return Component.GetOrCreateDataProvider(
                $"\"{itemsName}\" {ProviderName}",
                () => GetAll(extraXPath, itemsName));
        }

        private static FindAttribute ResolveItemFindAttribute()
        {
            return new FindControlListItemAttribute();
        }

        /// <summary>
        /// Gets the controls count.
        /// </summary>
        /// <returns>The count of controls.</returns>
        protected virtual int GetCount()
        {
            return GetItemElements().Count;
        }

        /// <summary>
        /// Gets the controls contents.
        /// </summary>
        /// <returns>The contents of controls.</returns>
        protected virtual IEnumerable<string> GetContents()
        {
            return GetAll().Select(x => (string)x.Content);
        }

        protected TItem GetItemByIndex(int index)
        {
            string itemName = (index + 1).Ordinalize();

            TItem DoGetItemByIndex() =>
                CreateItem(itemName, new FindByIndexAttribute(index));

            return _cachedNamedItemsMap.GetOrAdd(itemName, DoGetItemByIndex);
        }

        protected TItem GetItemByInnerXPath(string itemName, string xPath)
        {
            TItem DoGetItemByInnerXPath() =>
                CreateItem(itemName, new FindByInnerXPathAttribute(xPath));

            return _cachedNamedItemsMap.GetOrAdd(itemName, DoGetItemByInnerXPath);
        }

        protected virtual TItem GetItem(string name, Expression<Func<TItem, bool>> predicateExpression)
        {
            TItem DoGetItem()
            {
                var predicate = predicateExpression.Compile();

                ControlListScopeLocator scopeLocator = new ControlListScopeLocator(
                    searchOptions => GetItemElements(searchOptions)
                        .Where((element, index) => predicate(GetOrCreateItemByElement(element, (index + 1).Ordinalize()))));

                return CreateItem(scopeLocator, name);
            }

            return _cachedNamedItemsMap.GetOrAdd(name, DoGetItem);
        }

        /// <summary>
        /// Searches for the item that matches the conditions defined by the specified predicate expression
        /// and returns the zero-based index of the first occurrence.
        /// </summary>
        /// <param name="predicateExpression">The predicate expression to test each item.</param>
        /// <returns>The zero-based index of the first occurrence of item, if found; otherwise, <c>–1</c>.</returns>
        public DataProvider<int, TOwner> IndexOf(Expression<Func<TItem, bool>> predicateExpression)
        {
            predicateExpression.CheckNotNull(nameof(predicateExpression));

            string itemName = UIComponentResolver.ResolveControlName<TItem, TOwner>(predicateExpression);

            return Component.GetOrCreateDataProvider(
                $"{ComponentPartName} index of \"{itemName}\" {ItemComponentTypeName}",
                () => IndexOf(itemName, predicateExpression));
        }

        protected virtual int IndexOf(string name, Expression<Func<TItem, bool>> predicateExpression)
        {
            var predicate = predicateExpression.Compile();

            return GetItemElements().
                Select((element, index) => new { Element = element, Index = index }).
                Where(x => predicate(GetOrCreateItemByElement(x.Element, name))).
                Select(x => (int?)x.Index).
                FirstOrDefault() ?? -1;
        }

        [Obsolete("This method is not used anymore, no sense to invoke or override it.")] // Obsolete since v1.5.0.
        protected virtual By CreateItemBy()
        {
            FindAttribute itemFindAttribute = ResolveItemFindAttribute();
            itemFindAttribute.Properties.Metadata = Metadata;

            string outerXPath = itemFindAttribute.OuterXPath ?? ".//";

            By by = By.XPath($"{outerXPath}{ItemDefinition.ScopeXPath}").OfKind(ItemComponentTypeName);

            // TODO: Review/remake this Visibility processing.
            if (itemFindAttribute.Visibility == Visibility.Any)
                by = by.OfAnyVisibility();
            else if (itemFindAttribute.Visibility == Visibility.Hidden)
                by = by.Hidden();

            return by;
        }

        protected TItem GetOrCreateItemByElement(IWebElement element, string name)
        {
            TItem DoGetOrCreateItemByElement() =>
                CreateItem(new DefinedScopeLocator(element), name);

            TItem item = _cachedElementItemsMap.GetOrAdd(element, DoGetOrCreateItemByElement);
            item.Metadata.RemoveAll(x => x is NameAttribute);
            item.Metadata.Push(new NameAttribute(name));
            return item;
        }

        protected virtual TItem CreateItem(string name, params Attribute[] attributes)
        {
            var itemAttributes = new Attribute[] { new NameAttribute(name) }.Concat(
                attributes?.Concat(GetItemDeclaredAttributes()) ?? GetItemDeclaredAttributes());

            return CreateItem(itemAttributes);
        }

        protected TItem CreateItem(IScopeLocator scopeLocator, string name)
        {
            TItem item = CreateItem(name);

            if (scopeLocator is ControlListScopeLocator controlListScopeLocator)
                controlListScopeLocator.ElementName = item.ComponentFullName;

            item.ScopeLocator = scopeLocator;

            return item;
        }

        private TItem CreateItem(IEnumerable<Attribute> itemAttributes) =>
            Component.Find<TItem>(Metadata.Name, itemAttributes.ToArray());

        protected virtual IEnumerable<Attribute> GetItemDeclaredAttributes()
        {
            yield return ResolveItemFindAttribute();

            foreach (var item in Metadata.DeclaredAttributes)
                yield return item;
        }

        /// <summary>
        /// Selects the specified data (property) set of each control.
        /// Data can be a sub-control, an instance of <see cref="DataProvider{TData, TOwner}"/>, etc.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="selector">The data selector.</param>
        /// <returns>An instance of <see cref="DataProvider{TData, TOwner}"/>.</returns>
        public DataProvider<IEnumerable<TData>, TOwner> SelectData<TData>(Expression<Func<TItem, TData>> selector)
        {
            string dataPathName = ObjectExpressionStringBuilder.ExpressionToString(selector);

            return Component.GetOrCreateDataProvider(
                $"\"{dataPathName}\" {ProviderName}",
                () => GetAll().Select(selector.Compile()));
        }

        /// <summary>
        /// Selects the data of each control using JavaScript path relative to control element.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="elementValueJSPath">
        /// The JavaScript path to the element value,
        /// for example: <c>getAttribute('data-id')</c>.
        /// </param>
        /// <param name="dataProviderName">Name of the data provider to use in reporting.</param>
        /// <param name="valueTermOptions">The term options of value.</param>
        /// <returns>An instance of <see cref="DataProvider{TData, TOwner}"/>.</returns>
        public DataProvider<IEnumerable<TData>, TOwner> SelectData<TData>(
            string elementValueJSPath,
            string dataProviderName = null,
            TermOptions valueTermOptions = null) =>
            SelectDataByExtraXPath<TData>(null, elementValueJSPath, dataProviderName, valueTermOptions);

        /// <summary>
        /// Selects the data of each control using JavaScript path relative to element
        /// that is found using additional <paramref name="elementXPath"/>.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="elementXPath">The element XPath.</param>
        /// <param name="elementValueJSPath">
        /// The JavaScript path to the element value,
        /// for example: <c>getAttribute('data-id')</c>.
        /// </param>
        /// <param name="dataProviderName">Name of the data provider to use in reporting.</param>
        /// <param name="valueTermOptions">The term options of value.</param>
        /// <returns>An instance of <see cref="DataProvider{TData, TOwner}"/>.</returns>
        public DataProvider<IEnumerable<TData>, TOwner> SelectDataByExtraXPath<TData>(
            string elementXPath,
            string elementValueJSPath,
            string dataProviderName = null,
            TermOptions valueTermOptions = null)
        {
            elementValueJSPath.CheckNotNullOrEmpty(nameof(elementValueJSPath));

            if (dataProviderName == null)
            {
                StringBuilder nameBuilder = new StringBuilder();

                if (elementXPath != null)
                    nameBuilder.Append($"XPath: '{elementXPath}', ");

                nameBuilder.Append($"JSPath: '{elementValueJSPath}'");
                dataProviderName = nameBuilder.ToString();
            }

            return Component.GetOrCreateDataProvider(
                $"\"{dataProviderName}\" of {ProviderName}",
                () => SelectElementValues<TData>(elementXPath, elementValueJSPath, valueTermOptions));
        }

        /// <summary>
        /// Selects the content of each control relative to element
        /// that is found using additional <paramref name="elementXPath"/>.
        /// </summary>
        /// <param name="elementXPath">The element XPath.</param>
        /// <param name="dataProviderName">Name of the data provider to use in reporting.</param>
        /// <param name="valueTermOptions">The term options of value.</param>
        /// <returns>An instance of <see cref="DataProvider{TData, TOwner}"/>.</returns>
        public DataProvider<IEnumerable<string>, TOwner> SelectContentsByExtraXPath(
            string elementXPath,
            string dataProviderName = null,
            TermOptions valueTermOptions = null)
        {
            return SelectContentsByExtraXPath<string>(elementXPath, dataProviderName, valueTermOptions);
        }

        /// <summary>
        /// Selects the content of each control relative to element
        /// that is found using additional <paramref name="elementXPath"/>.
        /// </summary>
        /// <typeparam name="TData">The type of the data.</typeparam>
        /// <param name="elementXPath">The element XPath.</param>
        /// <param name="dataProviderName">Name of the data provider to use in reporting.</param>
        /// <param name="valueTermOptions">The term options of value.</param>
        /// <returns>An instance of <see cref="DataProvider{TData, TOwner}"/>.</returns>
        public DataProvider<IEnumerable<TData>, TOwner> SelectContentsByExtraXPath<TData>(
            string elementXPath,
            string dataProviderName = null,
            TermOptions valueTermOptions = null)
        {
            return SelectDataByExtraXPath<TData>(elementXPath, "textContent", dataProviderName, valueTermOptions);
        }

        protected IEnumerable<TData> SelectElementValues<TData>(
            string elementXPath,
            string elementValueJSPath,
            TermOptions valueTermOptions)
        {
            var elements = GetItemElements(extraXPath: elementXPath);

            return GetElementTextValues(elements, elementValueJSPath).
                Select(x => TermResolver.FromString<TData>(x, valueTermOptions));
        }

        private IEnumerable<string> GetElementTextValues(
            IEnumerable<IWebElement> elements,
            string elementValueJSPath)
        {
            string formattedScript = GetElementValuesScript.Replace("{0}", elementValueJSPath);

            return Component.Script.Execute<IEnumerable<object>>(formattedScript, elements).
                Value.
                Cast<string>().
                Select(x => x?.Trim()).
                ToArray();
        }

        /// <inheritdoc cref="GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<TItem> GetEnumerator() =>
            GetAll().GetEnumerator();

        protected virtual IEnumerable<TItem> GetAll() =>
            GetAll(null, null);

        protected virtual IEnumerable<TItem> GetAll(string extraXPath, string nameSuffix)
        {
            string nameFormat = string.IsNullOrEmpty(nameSuffix)
                ? "{0}"
                : $"{{0}} of {nameSuffix}";

            return GetItemElements(extraXPath: extraXPath).
                Select((element, index) => GetOrCreateItemByElement(element, string.Format(nameFormat, (index + 1).Ordinalize()))).
                ToArray();
        }

        [Obsolete("Use GetItemElements() or GetItemElements(SearchOptions, string) instead.")] // Obsolete since v1.5.0.
        protected ReadOnlyCollection<IWebElement> GetItemElements(By itemBy)
        {
            return GetItemElements();
        }

        protected ReadOnlyCollection<IWebElement> GetItemElements(SearchOptions searchOptions = null, string extraXPath = null)
        {
            searchOptions = searchOptions ?? ResolveSearchOptions();

            ReadOnlyCollection<IWebElement> DoGetItemElements()
            {
                TItem control = CreateItem(GetItemDeclaredAttributes());

                return control.ScopeLocator.GetElements(searchOptions, extraXPath).ToReadOnly();
            }

            return UseScopeCache
                ? _cachedAllElementsMap.GetOrAdd((searchOptions.Visibility, extraXPath), DoGetItemElements)
                : DoGetItemElements();
        }

        // TODO: Resolve visibility.
        private SearchOptions ResolveSearchOptions() =>
            new SearchOptions();

        void IClearsCache.ClearCache() =>
            ClearCache();

        /// <summary>
        /// Clears the cache of the controls.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner ClearCache()
        {
            if (_cachedAllElementsMap.Count > 0)
            {
                _cachedAllElementsMap.Clear();
                Component.Owner.Log.Trace($"Cleared scope cache of {Component.ComponentFullName} {ComponentPartName}");
            }

            if (_cachedNamedItemsMap.Count > 0)
            {
                foreach (var item in _cachedNamedItemsMap.Values)
                    item.ClearCache();

                _cachedNamedItemsMap.Clear();
            }

            if (_cachedElementItemsMap.Count > 0)
            {
                foreach (var item in _cachedElementItemsMap.Values)
                    item.ClearCache();

                _cachedElementItemsMap.Clear();
            }

            return Component.Owner;
        }
    }
}
