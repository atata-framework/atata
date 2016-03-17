using OpenQA.Selenium;
using System;
using System.Linq;

namespace Atata
{
    [UIComponent("input[@type='radio']", IgnoreNameEndings = "RadioButtons,RadioButtonList,Radios,RadioGroup,Buttons,ButtonList,Options,OptionGroup")]
    public class RadioButtonList<T, TOwner> : OptionList<T, TOwner>
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
                throw new ArgumentNullException("value", "Cannot set 'null' to RadioButtonList control.");

            IWebElement element = GetItemElement(value);
            if (!element.Selected)
                element.Click();
        }
    }
}
