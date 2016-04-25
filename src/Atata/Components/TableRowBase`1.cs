using OpenQA.Selenium;

namespace Atata
{
    [ControlDefinition("tr", ComponentTypeName = "table row")]
    public class TableRowBase<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected internal int? ColumnIndexToClick { get; set; }
        protected internal bool GoTemporarily { get; set; }

        protected IWebElement GetCell(int index)
        {
            return Scope.Get(By.XPath(".//td[{0}]").TableColumn().FormatWith(index + 1));
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            if (ColumnIndexToClick == null)
            {
                var columnIndexToClickAttribute = metadata.GetFirstOrDefaultComponentAttribute<ColumnIndexToClickAttribute>();
                if (columnIndexToClickAttribute != null)
                    ColumnIndexToClick = columnIndexToClickAttribute.Index;
            }
        }
    }
}
