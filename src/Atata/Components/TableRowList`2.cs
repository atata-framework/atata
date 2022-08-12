using System.Linq;

namespace Atata
{
    public class TableRowList<TItem, TOwner> : ControlList<TItem, TOwner>
        where TItem : TableRow<TOwner>
        where TOwner : PageObject<TOwner>
    {
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

        protected string BuildItemNameByCellValues(string[] values)
        {
            if (values == null || !values.Any())
                return null;
            else if (values.Length == 1)
                return values.First();
            else
                return values.ToQuotedValuesListOfString(true);
        }

        protected string CreateItemInnerXPathByCellValues(params string[] values)
        {
            return string.Join(" and ", values.Select(x => "td[{0}]".FormatWith(TermMatch.Contains.CreateXPathCondition(x))));
        }
    }
}
