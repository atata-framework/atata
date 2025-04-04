#nullable enable

namespace Atata;

/// <summary>
/// Represents the list of <c>&lt;tr&gt;</c> table row components.
/// </summary>
/// <typeparam name="TItem">The type of the row component.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public class TableRowList<TItem, TOwner> : ControlList<TItem, TOwner>
    where TItem : TableRow<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the row with the specified cell values.
    /// </summary>
    /// <value>
    /// The <typeparamref name="TItem"/> component.
    /// </value>
    /// <param name="cellValues">The cell values.</param>
    /// <returns>A row component.</returns>
    public TItem this[params string[] cellValues]
    {
        get
        {
            cellValues.CheckNotNullOrEmpty(nameof(cellValues));

            string itemName = BuildItemNameByCellValues(cellValues);
            string xPath = CreateItemInnerXPathByCellValues(cellValues);

            return GetItemByInnerXPath(itemName, xPath);
        }
    }

    protected static string BuildItemNameByCellValues(string[] values)
    {
        values.CheckNotNullOrEmpty(nameof(values));

        return values.ToDoubleQuotedValuesListOfString();
    }

    protected static string CreateItemInnerXPathByCellValues(params string[] values) =>
        string.Join(" and ", values.Select(x => "td[{0}]".FormatWith(TermMatch.Contains.CreateXPathCondition(x))));
}
