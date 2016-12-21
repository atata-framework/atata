using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the table row component (&lt;tr&gt;). Default search is performed by the content. By default every its control of type (or inherited from) <see cref="Content{T, TOwner}" /> is searched by the column header.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("tr[parent::table or parent::tbody]", ComponentTypeName = "row")]
    [ControlFinding(FindTermBy.Content)]
    [ControlFinding(FindTermBy.ColumnHeader, ControlType = typeof(Content<,>))]
    public class TableRow<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected IWebElement GetCell(int index)
        {
            return Scope.Get(By.XPath($".//td[{index + 1}]").TableColumn());
        }

        protected override void OnClick()
        {
            var columnIndexToClickAttribute = Parent.Metadata.Get<CellIndexToClickAttribute>(AttributeLevels.DeclaredAndComponent);

            if (columnIndexToClickAttribute != null)
            {
                GetCell(columnIndexToClickAttribute.Index).Click();
            }
            else
            {
                base.OnClick();
            }
        }
    }
}
