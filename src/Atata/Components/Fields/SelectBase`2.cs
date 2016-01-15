using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Atata
{
    [UIComponent("select")]
    public abstract class SelectBase<T, TOwner> : EditableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected SelectBase()
        {
        }

        protected SelectSettingsAttribute Settings { get; set; }

        protected string GetSelectedOptionValue()
        {
            IWebElement option = new SelectElement(Scope).SelectedOption;
            return Settings.By == SelectSelectionKind.ByText ? option.Text : option.GetValue();
        }

        protected void SetSelectedOptionValue(string value)
        {
            SelectElement selectElement = new SelectElement(Scope);
            if (Settings.By == SelectSelectionKind.ByText)
                selectElement.SelectByText(value);
            else
                selectElement.SelectByValue(value);
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            Settings = metadata.GetFirstOrDefaultPropertyAttribute<SelectSettingsAttribute>()
                ?? new SelectSettingsAttribute();
        }
    }
}
