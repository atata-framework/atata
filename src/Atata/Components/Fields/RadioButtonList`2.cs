using System.Linq;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the radio button list control (a set of <c>&lt;input type="radio"&gt;</c>).
    /// Default search is performed by the name.
    /// Specific radio button items can be found by label or value.
    /// By default items are searched by label using <see cref="FindItemByLabelAttribute"/>.
    /// Use <see cref="FindItemByValueAttribute"/> to find items by value.
    /// As a data type supports enum, string, numeric and other types.
    /// </summary>
    /// <typeparam name="T">
    /// The type of the control's data.
    /// Supports enum, string, numeric and other types.
    /// </typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButtons,RadioButtonList,Radios,RadioGroup,Buttons,ButtonList,Options,OptionGroup")]
    [ControlFinding(FindTermBy.Name)]
    [FindItemByLabel]
    public class RadioButtonList<T, TOwner> : OptionList<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override T GetValue()
        {
            IWebElement selectedItem = GetItemElements().FirstOrDefault(x => x.Selected);

            return selectedItem != null
                ? GetElementValue(selectedItem)
                : default(T);
        }

        protected override void SetValue(T value)
        {
            value.CheckNotNull(nameof(value), $"Cannot set \"null\" value to {nameof(RadioButtonList<T, TOwner>)} control.");

            IWebElement element = GetItemElement(value);
            if (!element.Selected)
                element.Click();
        }
    }
}
