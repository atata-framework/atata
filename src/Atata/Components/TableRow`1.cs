using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the table row component (&lt;tr&gt;).
    /// Default search finds the first occurring &lt;tr&gt; element.
    /// By default every its control of type (or inherited from) <see cref="Content{TValue, TOwner}" /> is searched by the column header.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("tr[parent::table or parent::tbody]", ComponentTypeName = "row")]
    [FindByColumnHeader(TargetType = typeof(Content<,>))]
    public class TableRow<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected IWebElement GetCell(int index) =>
            Scope.GetWithLogging(By.XPath($".//td[{index + 1}]").TableColumn());
    }
}
