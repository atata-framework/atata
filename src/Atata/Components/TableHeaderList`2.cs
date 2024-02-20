namespace Atata;

/// <summary>
/// Represents the list of <c>&lt;th&gt;</c> table header components.
/// </summary>
/// <typeparam name="TItem">The type of the header component.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public class TableHeaderList<TItem, TOwner> : ControlList<TItem, TOwner>
    where TItem : TableHeader<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the header with the specified text.
    /// </summary>
    /// <value>
    /// The <typeparamref name="TItem"/> component.
    /// </value>
    /// <param name="headerText">The header text.</param>
    /// <returns>A header component.</returns>
    public TItem this[string headerText]
    {
        get
        {
            headerText.CheckNotNull(nameof(headerText));

            string xPath = TermMatch.Contains.CreateXPathCondition(headerText);

            return GetItemByInnerXPath($"\"{headerText}\"", xPath);
        }
    }
}
