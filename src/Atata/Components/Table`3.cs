using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the table control (&lt;table&gt;).
    /// Default search finds the first occurring &lt;table&gt; element.
    /// </summary>
    /// <typeparam name="THeader">The type of the table header control.</typeparam>
    /// <typeparam name="TRow">The type of the table row control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("table", IgnoreNameEndings = "Table", ComponentTypeName = "table")]
    [UseColumnHeaderTextsCache]
    public class Table<THeader, TRow, TOwner> : Control<TOwner>, ITable
        where THeader : TableHeader<TOwner>
        where TRow : TableRow<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private List<string> _cachedColumnHeaderTexts;

        /// <summary>
        /// Gets the rows list.
        /// </summary>
        public TableRowList<TRow, TOwner> Rows { get; private set; }

        /// <summary>
        /// Gets the headers list.
        /// </summary>
        public ControlList<THeader, TOwner> Headers { get; private set; }

        /// <summary>
        /// Gets a value indicating whether to the cache of column header texts.
        /// Returns a <see cref="UseColumnHeaderTextsCacheAttribute.UseCache"/> value of
        /// an associated with the component <see cref="UseColumnHeaderTextsCacheAttribute"/>.
        /// The default value is <see langword="true"/>.
        /// </summary>
        protected bool UseColumnHeaderTextsCache =>
            Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute || x is UseColumnHeaderTextsCacheAttribute))
                ?.UseCache ?? false;

        /// <inheritdoc/>
        public IEnumerable<string> GetColumnHeaderTexts()
        {
            bool useColumnHeaderTextsCache = UseColumnHeaderTextsCache;

            if (useColumnHeaderTextsCache && (_cachedColumnHeaderTexts?.Any() ?? false))
                return _cachedColumnHeaderTexts;

            var columnHeaderTexts = Log.ExecuteSection(
                new LogSection($"Select column header texts of {ComponentFullName}", LogLevel.Trace),
                SelectColumnHeaderTexts);

            if (useColumnHeaderTextsCache)
                _cachedColumnHeaderTexts = new List<string>(columnHeaderTexts);

            return columnHeaderTexts;
        }

        /// <summary>
        /// Selects the column header texts.
        /// </summary>
        /// <returns>The collection of text values.</returns>
        protected virtual IEnumerable<string> SelectColumnHeaderTexts() =>
            Headers.SelectContentsByExtraXPath(elementXPath: null, dataProviderName: "column header texts").Value.ToArray();

        /// <summary>
        /// Clears the column header texts of the component.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner ClearColumnHeaderTextsCache()
        {
            if (_cachedColumnHeaderTexts?.Any() ?? false)
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
}
