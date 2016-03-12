using OpenQA.Selenium;
using System;
using System.Linq;

namespace Atata
{
    [UIComponent("input[@type='radio']", IgnoreNameEndings = "RadioButtons,RadioButtonGroup,Radios,RadioGroup,Buttons,ButtonGroup,Options,OptionGroup")]
    public class RadioButtonGroup<TEnum, TOwner> : GroupField<TEnum, TOwner>
        where TEnum : struct, IComparable, IFormattable
        where TOwner : PageObject<TOwner>
    {
        protected override TEnum GetValue()
        {
            IWebElement selectedItem = GetItemElements().FirstOrDefault(x => x.Selected);
            if (selectedItem != null)
                return ItemElementFindStrategy.GetParameter<TEnum>(selectedItem);
            else
                return default(TEnum);
        }

        protected override void SetValue(TEnum value)
        {
            IWebElement element = GetItemElement(value);
            if (!element.Selected)
                element.Click();
        }
    }
}
