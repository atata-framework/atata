using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Atata
{
    /// <summary>
    /// Represents the select control (&lt;select&gt;). Property can be marked with <see cref="SelectByValueAttribute"/> or <see cref="SelectByTextAttribute"/>. Default selection is performed by text.
    /// </summary>
    /// <typeparam name="T">The type of the data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("select", IgnoreNameEndings = "Select")]
    public class Select<T, TOwner> : EditableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private TermSettingsAttribute selectByAttribute;
        private SelectBy by;

        private enum SelectBy
        {
            Text,
            Value
        }

        protected override T GetValue()
        {
            IWebElement option = new SelectElement(Scope).SelectedOption;
            string valueAsString = by == SelectBy.Text ? option.Text : option.GetValue();
            return ConvertStringToValue(valueAsString);
        }

        protected override void SetValue(T value)
        {
            string valueAsString = ConvertValueToString(value);
            SelectElement selectElement = new SelectElement(Scope);

            if (by == SelectBy.Text)
                selectElement.SelectByText(valueAsString);
            else
                selectElement.SelectByValue(valueAsString);
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            selectByAttribute = metadata.GetFirstOrDefaultDeclaringAttribute<SelectByTextAttribute>()
                ?? (TermSettingsAttribute)metadata.GetFirstOrDefaultDeclaringAttribute<SelectByValueAttribute>()
                ?? new SelectByTextAttribute();

            by = selectByAttribute is SelectByTextAttribute ? SelectBy.Text : SelectBy.Value;

            base.ApplyMetadata(metadata);
        }

        protected override void InitValueTermOptions(TermOptions termOptions, UIComponentMetadata metadata)
        {
            base.InitValueTermOptions(termOptions, metadata);

            termOptions.MergeWith(selectByAttribute);
        }
    }
}
