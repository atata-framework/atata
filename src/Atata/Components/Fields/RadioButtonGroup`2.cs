using OpenQA.Selenium;
using System;
using System.Linq;

namespace Atata
{
    [UIComponent("input[@type='radio']", IgnoreNameEndings = "RadioButtons,RadioButtonGroup,Radios,RadioGroup,Buttons,ButtonGroup,Options,OptionGroup")]
    public class RadioButtonGroup<T, TOwner> : GroupField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override T GetValue()
        {
            IWebElement selectedItem = GetItemElements().FirstOrDefault(x => x.Selected);
            if (selectedItem != null)
                return ItemElementFindStrategy.GetParameter<T>(selectedItem, ValueTermOptions);
            else
                return default(T);
        }

        protected override void SetValue(T value)
        {
            if (value == null)
                throw new ArgumentNullException("value", "Cannot set 'null' to RadioButtonGroup control.");

            IWebElement element = GetItemElement(value);
            if (!element.Selected)
                element.Click();
        }
    }
}
