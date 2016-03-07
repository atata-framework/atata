using Humanizer;
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
            IWebElement selectedItem = GetItems().FirstOrDefault(x => x.Selected);
            if (selectedItem != null)
            {
                string stringValue = ((IItemsControl)this).ItemFindStrategy.GetParameter(selectedItem).ToString();
                Enum dehumanizedValue = stringValue.DehumanizeTo(typeof(TEnum), OnNoMatch.ReturnsNull);
                if (dehumanizedValue != null)
                    return (TEnum)(object)dehumanizedValue;
                else
                    return (TEnum)Enum.Parse(typeof(TEnum), stringValue);
            }
            else
            {
                return default(TEnum);
            }
        }

        protected override void SetValue(TEnum value)
        {
            IWebElement element = GetItem(value);
            if (!element.Selected)
                element.Click();
        }
    }
}
