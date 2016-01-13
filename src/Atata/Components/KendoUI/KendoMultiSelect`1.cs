using OpenQA.Selenium;

namespace Atata.KendoUI
{
    [UIComponent("div[contains(concat(' ', normalize-space(@class), ' '), ' k-multiselect ')]", "div[div[1]/ul[id='{0}_taglist']]")]
    public class KendoMultiSelect<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public TOwner Add(string value)
        {
            Log.StartAddingSection(ComponentName, value);

            var input = Scope.Get(By.CssSelector("input.k-input"));
            input.Click();
            input.FillInWith(value);

            Scope.Get(By.XPath("//body")).
                Get(By.CssSelector(".k-animation-container .k-list-container.k-popup").DropDownList()).
                Get(By.XPath(".//li[contains(.,'{0}')]").DropDownOption(value));

            input.SendKeys(Keys.Enter);

            Scope.Get(By.XPath(".//ul/li[contains(.,'{0}')]").Named(value).OfKind("value in control"));

            Log.EndSection();

            return Owner;
        }
    }
}
