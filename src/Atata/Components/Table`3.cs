namespace Atata;

/// <summary>
/// Represents the table control (&lt;table&gt;).
/// Default search finds the first occurring &lt;table&gt; element.
/// </summary>
/// <typeparam name="THeader">The type of the table header control.</typeparam>
/// <typeparam name="TRow">The type of the table row control.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("table", IgnoreNameEndings = "Table", ComponentTypeName = "table")]
public class Table<THeader, TRow, TOwner> : Control<TOwner>, ITable
    where THeader : TableHeader<TOwner>
    where TRow : TableRow<TOwner>
    where TOwner : PageObject<TOwner>
{
    private List<string>? _cachedColumnHeaderTexts;

    /// <summary>
    /// Gets the rows list.
    /// </summary>
    public TableRowList<TRow, TOwner> Rows { get; private set; } = null!;

    /// <summary>
    /// Gets the headers list.
    /// </summary>
    public TableHeaderList<THeader, TOwner> Headers { get; private set; } = null!;

    /// <summary>
    /// Gets a value indicating whether to the cache of column header texts.
    /// Returns a <see cref="ICanUseCache.UsesCache"/> value of an associated with the component
    /// <see cref="UsesColumnHeaderTextsCacheAttribute"/> or <see cref="UsesCacheAttribute"/>.
    /// The default value is <see langword="true"/>.
    /// </summary>
    protected bool UsesColumnHeaderTextsCache =>
        Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute or UsesColumnHeaderTextsCacheAttribute))
            ?.UsesCache ?? true;

    /// <inheritdoc/>
    public IEnumerable<string> GetColumnHeaderTexts()
    {
        bool usesColumnHeaderTextsCache = UsesColumnHeaderTextsCache;

        if (usesColumnHeaderTextsCache && _cachedColumnHeaderTexts?.Count > 0)
            return _cachedColumnHeaderTexts;

        var columnHeaderTexts = Log.ExecuteSection(
            new LogSection($"Select column header texts of {ComponentFullName}", LogLevel.Trace),
            SelectColumnHeaderTexts);

        if (usesColumnHeaderTextsCache)
            _cachedColumnHeaderTexts = [.. columnHeaderTexts];

        return columnHeaderTexts;
    }

    /// <summary>
    /// Selects the column header texts.
    /// </summary>
    /// <returns>The collection of text values.</returns>
    protected virtual IEnumerable<string> SelectColumnHeaderTexts() =>
        Headers.SelectContentsByExtraXPath(elementXPath: null, valueProviderName: "column header texts").Value.ToArray();

    /// <summary>
    /// Clears the column header texts of the component.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner ClearColumnHeaderTextsCache()
    {
        if (_cachedColumnHeaderTexts?.Count > 0)
        {
            _cachedColumnHeaderTexts = null;
            Log.Trace($"Cleared column header texts cache of {ComponentFullName}");
        }

        return Owner;
    }

    protected override void OnClearCache()
    {
        base.OnClearCache();

        ClearColumnHeaderTextsCache();
    }
}
