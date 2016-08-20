using OpenQA.Selenium;

namespace Atata
{
    [ControlDefinition("tr[parent::table or parent::tbody]", ComponentTypeName = "row")]
    public class TableRowBase<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected internal bool GoTemporarily { get; set; }

        protected IWebElement GetCell(int index)
        {
            return Scope.Get(By.XPath($".//td[{index + 1}]").TableColumn());
        }

        protected override void OnClick()
        {
            var columnIndexToClickAttribute = Parent.Metadata.GetFirstOrDefaultDeclaringAttribute<CellIndexToClickAttribute>()
                ?? Metadata.GetFirstOrDefaultComponentAttribute<CellIndexToClickAttribute>();

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
