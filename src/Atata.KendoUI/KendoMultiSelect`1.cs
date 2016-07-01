using OpenQA.Selenium;

namespace Atata.KendoUI
{
    [ControlDefinition(
        "div",
        ContainingClass = "k-multiselect",
        ComponentTypeName = "multi-select",
        IdXPathFormat = "div[div[1]/ul[id='{0}_taglist']]")]
    [ValueXPath("span[1]")]
    public class KendoMultiSelect<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected string ValueXPath
        {
            get { return Metadata.GetFirstOrDefaultDeclaringOrComponentAttribute<ValueXPathAttribute>()?.XPath; }
        }

        protected string ItemValueXPath
        {
            get { return Metadata.GetFirstOrDefaultDeclaringOrComponentAttribute<ItemValueXPathAttribute>()?.XPath; }
        }

        public TOwner Add(string value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.StartAddingSection(ComponentFullName, value);

            OnAdd(value);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        protected virtual void OnAdd(string value)
        {
            var input = Scope.Get(By.CssSelector("input.k-input"));
            input.FillInWith(value);

            Scope.Get(By.XPath("//body")).
                Get(By.CssSelector(".k-animation-container .k-list-container.k-popup").DropDownList()).
                Get(By.XPath(".//li{0}[.='{1}')]").FormatWith(ItemValueXPath, value).DropDownOption(value));

            input.SendKeys(Keys.Enter);

            Scope.Get(By.XPath(".//ul/li{0}[.='{1}')]").FormatWith(ValueXPath, value).OfKind("value in control", value));
        }
    }
}
