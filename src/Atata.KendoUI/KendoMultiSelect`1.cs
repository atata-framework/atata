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
        [FindByClass("k-input")]
        protected virtual TextInput<TOwner> Input { get; set; }

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
            Input.Set(value);

            GetDropDownOption(value);

            Driver.Perform(x => x.SendKeys(Keys.Enter));

            Scope.Get(By.XPath(".//ul/li{0}[normalize-space(.)='{1}']").FormatWith(ValueXPath, value).OfKind("added item element", value));
        }

        protected virtual IWebElement GetDropDownList()
        {
            return Driver.
                Get(By.CssSelector(".k-animation-container .k-list-container.k-popup").DropDownList());
        }

        protected virtual IWebElement GetDropDownOption(string value, SearchOptions searchOptions = null)
        {
            return GetDropDownList().
               Get(By.XPath(".//li{0}[normalize-space(.)='{1}']").FormatWith(ItemValueXPath, value).DropDownOption(value).With(searchOptions));
        }
    }
}
