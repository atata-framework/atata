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
    /// <typeparam name="TValue">
    /// The type of the control's value.
    /// Supports enum, string, numeric and other types.
    /// </typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButtons,RadioButtonList,Radios,RadioGroup,Buttons,ButtonList,Options,OptionGroup", ComponentTypeName = "radio button list")]
    [FindByName]
    [FindItemByLabel]
    public class RadioButtonList<TValue, TOwner> : OptionList<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override TValue GetValue()
        {
            IWebElement selectedItem = GetItemElements().FirstOrDefault(x => x.Selected);

            return selectedItem != null
                ? GetElementValue(selectedItem)
                : default;
        }

        protected override void SetValue(TValue value)
        {
            value.CheckNotNull(nameof(value), $"Cannot set \"null\" value to {nameof(RadioButtonList<TValue, TOwner>)} control.");

            IWebElement element = GetItemElement(value);
            if (!element.Selected)
                element.ClickWithLogging();
        }

        /// <summary>
        /// Sets random unselected value.
        /// </summary>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner ToggleRandom()
        {
            Log.ExecuteSection(
                new LogSection($"Toggle random {ComponentFullName}"),
                () =>
                {
                    var unselectedOptionElements = GetItemElements().Where(x => !x.Selected).ToArray();

                    IWebElement randomElement = Randomizer.GetOneOf(unselectedOptionElements);
                    randomElement.ClickWithLogging();
                });

            return Owner;
        }
    }
}
