namespace Atata;

/// <summary>
/// Represents the list of controls of <typeparamref name="TItem"/> type.
/// </summary>
/// <typeparam name="TItem">The type of the item control.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public class ControlList<TItem, TOwner> :
    UIComponentPart<TOwner>,
    ISupportsMetadata,
    IEnumerableProvider<TItem, TOwner>,
    IClearsCache
    where TItem : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
    private const string ItemText = "item";

    private const string ItemsText = "items";

    private const string ValuesText = "values";

    private const string ContentsText = "contents";

    private const string ElementsText = "elements";

    private const string ContentElementValueJSPath = "textContent";

    protected const string GetElementValuesScript = @"
var elements = arguments[0];
var textValues = [];

for (var i = 0; i < elements.length; i++) {
    textValues.push(elements[i].{0});
}

return textValues;";

    private readonly Dictionary<string, TItem> _cachedNamedItemsMap = [];

    private readonly Dictionary<(Visibility Visibility, string ExtraXPath), ReadOnlyCollection<IWebElement>> _cachedAllElementsMap = [];

    private readonly Dictionary<IWebElement, TItem> _cachedElementItemsMap = [];

    private string _itemComponentTypeName;

    protected string ItemComponentTypeName =>
        _itemComponentTypeName ??= UIComponentResolver.ResolveControlTypeName(Metadata);

    /// <inheritdoc cref="UIComponent{TOwner}.Should"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ObjectVerificationProvider<IEnumerable<TItem>, TOwner> Should =>
        new(this, Component.Session.ExecutionUnit);

    /// <inheritdoc cref="UIComponent{TOwner}.ExpectTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ObjectVerificationProvider<IEnumerable<TItem>, TOwner> ExpectTo =>
        Should.Using(ExpectationVerificationStrategy.Instance);

    /// <inheritdoc cref="UIComponent{TOwner}.WaitTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public ObjectVerificationProvider<IEnumerable<TItem>, TOwner> WaitTo =>
        Should.Using(WaitingVerificationStrategy.Instance);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of controls count.
    /// </summary>
    public ValueProvider<int, TOwner> Count =>
        Component.CreateValueProvider(
            BuildCountValueProviderFullName(),
            GetCount);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of controls contents.
    /// </summary>
    public ValueProvider<IEnumerable<string>, TOwner> Contents =>
        Component.CreateValueProvider(
            BuildValueProviderFullName("contents"),
            GetContents);

    protected string ProviderName =>
        IsComponentPageObject
            ? ComponentPartName
            : $"{Component.ComponentFullName}{UIComponent.SubComponentSeparator}{ComponentPartName}";

    private bool IsComponentPageObject =>
        Component.GetType().IsSubclassOfRawGeneric(typeof(PageObject<>));

    string IObjectProvider<IEnumerable<TItem>>.ProviderName => ProviderName;

    TOwner IObjectProvider<IEnumerable<TItem>, TOwner>.Owner => Component.Owner;

    IEnumerable<TItem> IObjectProvider<IEnumerable<TItem>>.Object => GetAll();

    bool IObjectProvider<IEnumerable<TItem>, TOwner>.IsDynamic => true;

    IAtataExecutionUnit IObjectProvider<IEnumerable<TItem>>.ExecutionUnit =>
        Component.Session.ExecutionUnit;

    UIComponentMetadata ISupportsMetadata.Metadata
    {
        get => Metadata;
        set => Metadata = value;
    }

    public UIComponentMetadata Metadata { get; private set; }

    Type ISupportsMetadata.ComponentType => typeof(TItem);

    protected bool UsesScopeCache =>
        Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute or UsesScopeCacheAttribute))
            ?.UsesCache ?? false;

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

        itemName = itemName is null
            ? $"[{xPathCondition}]"
            : WrapSubProviderNameWithDoubleQuotes(itemName);

        return GetItemByInnerXPath(itemName, xPathCondition);
    }

    /// <summary>
    /// Gets all controls of this list that match the specified XPath condition.
    /// </summary>
    /// <param name="xPathCondition">
    /// The XPath condition.
    /// For example: <c>"@some-attr='some value'"</c>.</param>
    /// <returns>All items that match the XPath condition.</returns>
    public EnumerableValueProvider<TItem, TOwner> GetAllByXPathCondition(string xPathCondition) =>
        GetAllByXPathCondition(null, xPathCondition);

    /// <summary>
    /// Gets all controls of this list that match the specified XPath condition.
    /// </summary>
    /// <param name="itemsName">Name of the items to use in reporting.</param>
    /// <param name="xPathCondition">
    /// The XPath condition.
    /// For example: <c>"@some-attr='some value'"</c>.</param>
    /// <returns>All items that match the XPath condition.</returns>
    public EnumerableValueProvider<TItem, TOwner> GetAllByXPathCondition(string itemsName, string xPathCondition)
    {
        xPathCondition.CheckNotNullOrEmpty(nameof(xPathCondition));

        string extraXPath = xPathCondition[0] == '['
            ? xPathCondition
            : $"[{xPathCondition}]";

        itemsName = itemsName is null
            ? extraXPath
            : WrapSubProviderNameWithDoubleQuotes(itemsName);

        return Component.CreateEnumerableValueProvider(
            BuildValueProviderFullName($"{itemsName} {ItemsText}"),
            () => GetAll(extraXPath, itemsName));
    }

    // TODO: Review ResolveItemFindAttribute.
    private static FindControlListItemAttribute ResolveItemFindAttribute() =>
        new();

    private static string WrapSubProviderNameWithDoubleQuotes(string name) =>
        name.Length > 0 && name[0] == '"'
            ? name
            : $"\"{name}\"";

    /// <summary>
    /// Gets the controls count.
    /// </summary>
    /// <returns>The count of controls.</returns>
    protected virtual int GetCount() =>
        GetItemElements().Count;

    /// <summary>
    /// Gets the controls contents.
    /// </summary>
    /// <returns>The contents of controls.</returns>
    protected virtual IEnumerable<string> GetContents() =>
        GetAll().Select(x => (string)x.Content);

    protected TItem GetItemByIndex(int index)
    {
        string itemName = (index + 1).Ordinalize();

        TItem DoGetItemByIndex() =>
            CreateItem(itemName, new FindByIndexAttribute(index));

        return UsesScopeCache
            ? _cachedNamedItemsMap.GetOrAdd(itemName, DoGetItemByIndex)
            : DoGetItemByIndex();
    }

    protected TItem GetItemByInnerXPath(string itemName, string xPath)
    {
        TItem DoGetItemByInnerXPath() =>
            CreateItem(itemName, new FindByInnerXPathAttribute(xPath));

        return UsesScopeCache
            ? _cachedNamedItemsMap.GetOrAdd(itemName, DoGetItemByInnerXPath)
            : DoGetItemByInnerXPath();
    }

    protected virtual TItem GetItem(string name, Expression<Func<TItem, bool>> predicateExpression)
    {
        var predicate = predicateExpression.Compile();

        ControlListScopeLocator scopeLocator = new ControlListScopeLocator(
            Component.Session,
            searchOptions => GetItemElements(searchOptions)
                .Where((element, index) => predicate(GetOrCreateItemByElement(element, (index + 1).Ordinalize()))));

        return CreateItem(scopeLocator, name);
    }

    /// <summary>
    /// Searches for the item that matches the conditions defined by the specified predicate expression
    /// and returns the zero-based index of the first occurrence.
    /// </summary>
    /// <param name="predicateExpression">The predicate expression to test each item.</param>
    /// <returns>
    /// The <see cref="ValueProvider{TValue, TOwner}"/> containing zero-based index
    /// of the first occurrence of item, if found; otherwise, <c>–1</c>.
    /// </returns>
    public ValueProvider<int, TOwner> IndexOf(Expression<Func<TItem, bool>> predicateExpression)
    {
        predicateExpression.CheckNotNull(nameof(predicateExpression));

        string itemName = UIComponentResolver.ResolveControlName<TItem, TOwner>(predicateExpression);

        return Component.CreateValueProvider(
            BuildValueProviderFullName($"{itemName} {ItemText} index"),
            () => IndexOf(itemName, predicateExpression));
    }

    protected virtual int IndexOf(string name, Expression<Func<TItem, bool>> predicateExpression)
    {
        var predicate = predicateExpression.Compile();

        return GetItemElements()
            .Select((element, index) => new { Element = element, Index = index })
            .Where(x => predicate(GetOrCreateItemByElement(x.Element, name)))
            .Select(x => (int?)x.Index)
            .FirstOrDefault() ?? -1;
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
        string fullName = $"{ComponentPartName}{UIComponent.SubComponentSeparator}{name} {ItemText}";

        var itemAttributes = new Attribute[] { new NameAttribute(fullName) }.Concat(
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

    private TItem CreateItem(IEnumerable<Attribute> itemAttributes)
    {
        var item = Component.Find<TItem>(Metadata.Name, itemAttributes.ToArray());
        item.IncludeComponentTypeNameInFullName = false;
        return item;
    }

    protected virtual IEnumerable<Attribute> GetItemDeclaredAttributes()
    {
        yield return ResolveItemFindAttribute();

        foreach (var item in Metadata.DeclaredAttributes)
            yield return item;
    }

    /// <summary>
    /// Selects the specified data (property) set of each control.
    /// Data can be a sub-control, an instance of <see cref="ValueProvider{TValue, TOwner}"/>, etc.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <param name="selector">The data selector.</param>
    /// <returns>An instance of <see cref="ValueProvider{TValue, TOwner}"/>.</returns>
    public ValueProvider<IEnumerable<TData>, TOwner> SelectData<TData>(Expression<Func<TItem, TData>> selector)
    {
        string dataPathName = ObjectExpressionStringBuilder.ExpressionToString(selector);

        return Component.CreateValueProvider(
            BuildValueProviderFullName($"\"{dataPathName}\" {ValuesText}"),
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
    /// <param name="valueProviderName">Name of the value provider to use in reporting.</param>
    /// <param name="valueTermOptions">The term options of value.</param>
    /// <returns>An instance of <see cref="ValueProvider{TValue, TOwner}"/>.</returns>
    public ValueProvider<IEnumerable<TData>, TOwner> SelectData<TData>(
        string elementValueJSPath,
        string valueProviderName = null,
        TermOptions valueTermOptions = null) =>
        SelectDataByExtraXPath<TData>(null, elementValueJSPath, valueProviderName, valueTermOptions);

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
    /// <param name="valueProviderName">Name of the value provider to use in reporting.</param>
    /// <param name="valueTermOptions">The term options of value.</param>
    /// <returns>An instance of <see cref="ValueProvider{TValue, TOwner}"/>.</returns>
    public ValueProvider<IEnumerable<TData>, TOwner> SelectDataByExtraXPath<TData>(
        string elementXPath,
        string elementValueJSPath,
        string valueProviderName = null,
        TermOptions valueTermOptions = null)
    {
        elementValueJSPath.CheckNotNullOrEmpty(nameof(elementValueJSPath));

        if (valueProviderName is null)
        {
            StringBuilder nameBuilder = new StringBuilder();

            if (elementXPath != null)
            {
                nameBuilder.Append($"XPath \"{elementXPath}\" {ElementsText}");

                if (elementValueJSPath != ContentElementValueJSPath)
                    nameBuilder.Append(UIComponent.SubComponentSeparator);
            }

            if (elementValueJSPath != ContentElementValueJSPath)
                nameBuilder.Append($"JSPath \"{elementValueJSPath}\"");

            valueProviderName = nameBuilder.ToString();
        }
        else
        {
            valueProviderName = WrapSubProviderNameWithDoubleQuotes(valueProviderName);
        }

        string valuesText = elementValueJSPath == ContentElementValueJSPath
            ? ContentsText
            : ValuesText;

        return Component.CreateValueProvider(
            BuildValueProviderFullName($"{valueProviderName} {valuesText}"),
            () => SelectElementValues<TData>(elementXPath, elementValueJSPath, valueTermOptions));
    }

    private string BuildValueProviderFullName(string name)
    {
        var builder = new StringBuilder();

        if (!IsComponentPageObject)
            builder.Append(UIComponent.SubComponentSeparator.TrimStart());

        return builder.Append(ComponentPartName)
            .Append(UIComponent.SubComponentSeparator)
            .Append(name)
            .ToString();
    }

    private string BuildCountValueProviderFullName()
    {
        var builder = new StringBuilder();

        if (!IsComponentPageObject)
            builder.Append(UIComponent.SubComponentSeparator.TrimStart());

        return builder.Append(ComponentPartName)
            .Append(" count")
            .ToString();
    }

    /// <summary>
    /// Selects the content of each control relative to element
    /// that is found using additional <paramref name="elementXPath"/>.
    /// </summary>
    /// <param name="elementXPath">The element XPath.</param>
    /// <param name="valueProviderName">Name of the value provider to use in reporting.</param>
    /// <param name="valueTermOptions">The term options of value.</param>
    /// <returns>An instance of <see cref="ValueProvider{TValue, TOwner}"/>.</returns>
    public ValueProvider<IEnumerable<string>, TOwner> SelectContentsByExtraXPath(
        string elementXPath,
        string valueProviderName = null,
        TermOptions valueTermOptions = null)
        =>
        SelectContentsByExtraXPath<string>(elementXPath, valueProviderName, valueTermOptions);

    /// <summary>
    /// Selects the content of each control relative to element
    /// that is found using additional <paramref name="elementXPath"/>.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <param name="elementXPath">The element XPath.</param>
    /// <param name="valueProviderName">Name of the value provider to use in reporting.</param>
    /// <param name="valueTermOptions">The term options of value.</param>
    /// <returns>An instance of <see cref="ValueProvider{TValue, TOwner}"/>.</returns>
    public ValueProvider<IEnumerable<TData>, TOwner> SelectContentsByExtraXPath<TData>(
        string elementXPath,
        string valueProviderName = null,
        TermOptions valueTermOptions = null)
        =>
        SelectDataByExtraXPath<TData>(elementXPath, ContentElementValueJSPath, valueProviderName, valueTermOptions);

    protected IEnumerable<TData> SelectElementValues<TData>(
        string elementXPath,
        string elementValueJSPath,
        TermOptions valueTermOptions)
    {
        var elements = GetItemElements(extraXPath: elementXPath);

        return GetElementTextValues(elements, elementValueJSPath)
            .Select(x => TermResolver.FromString<TData>(x, valueTermOptions));
    }

    private string[] GetElementTextValues(
        IEnumerable<IWebElement> elements,
        string elementValueJSPath)
    {
        string formattedScript = GetElementValuesScript.Replace("{0}", elementValueJSPath);

        return Component.Script.Execute<IEnumerable<object>>(formattedScript, elements)
            .Value.Cast<string>()
            .Select(x => x?.Trim())
            .ToArray();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the collection.</returns>
    public IEnumerator<TItem> GetEnumerator() =>
        GetAll().GetEnumerator();

    /// <summary>
    /// Returns the controls as <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <returns>The controls as <see cref="IEnumerable{T}"/>.</returns>
    public IEnumerable<TItem> AsEnumerable() =>
        GetAll();

    protected virtual IEnumerable<TItem> GetAll() =>
        GetAll(null, null);

    protected virtual IEnumerable<TItem> GetAll(string extraXPath, string itemsName)
    {
        string nameFormat = string.IsNullOrEmpty(itemsName)
            ? "{0}"
            : $"{itemsName} {ItemsText}{UIComponent.SubComponentSeparator}{{0}}";

        return GetItemElements(extraXPath: extraXPath)
            .Select((element, index) => GetOrCreateItemByElement(element, string.Format(nameFormat, (index + 1).Ordinalize())))
            .ToArray();
    }

    protected ReadOnlyCollection<IWebElement> GetItemElements(SearchOptions searchOptions = null, string extraXPath = null)
    {
        searchOptions ??= ResolveSearchOptions();

        ReadOnlyCollection<IWebElement> DoGetItemElements()
        {
            TItem control = CreateItem(GetItemDeclaredAttributes());

            return control.ScopeLocator.GetElements(searchOptions, extraXPath).ToReadOnly();
        }

        return UsesScopeCache
            ? _cachedAllElementsMap.GetOrAdd((searchOptions.Visibility, extraXPath), DoGetItemElements)
            : DoGetItemElements();
    }

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
            Component.Owner.Log.Trace($"Cleared scope cache of {ProviderName}");
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

    // TODO: Resolve visibility.
    private static SearchOptions ResolveSearchOptions() =>
        new();
}
