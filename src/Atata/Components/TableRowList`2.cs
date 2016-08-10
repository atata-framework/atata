using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    public class TableRowList<TItem, TOwner> : ControlList<TItem, TOwner>
        where TItem : TableRowBase<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public TItem this[params string[] cellValues]
        {
            get
            {
                cellValues.CheckNotNullOrEmpty(nameof(cellValues));

                string itemName = BuildItemName(cellValues);
                By itemBy = CreateItemBy(cellValues);

                return GetItem(itemName, itemBy);
            }
        }

        protected virtual string BuildItemName(string[] values)
        {
            if (values == null || !values.Any())
                return null;
            else if (values.Length == 1)
                return values.First();
            else
                return values.ToQuotedValuesListOfString();
        }

        protected virtual By CreateItemBy(params string[] values)
        {
            string condition = values != null && values.Any()
                ? string.Concat(values.Select(x => "[td[{0}]]".FormatWith(TermMatch.Contains.CreateXPathCondition(x))))
                : "[td]";

            return By.XPath(".//{0}{1}".FormatWith(ItemDefinition.ScopeXPath, condition)).OfKind(ItemDefinition.ComponentTypeName);
        }
    }
}
