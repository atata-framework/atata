using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the checkbox control (&lt;input type="checkbox"&gt;). Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='checkbox']", ComponentTypeName = "checkbox", IgnoreNameEndings = "Checkbox,CheckBox,Option")]
    public class CheckBox<TOwner> : EditableField<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}" /> instance of the checked state value.
        /// </summary>
        public DataProvider<bool, TOwner> IsChecked => GetOrCreateDataProvider("checked", () => Value);

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<bool, CheckBox<TOwner>, TOwner> Should => new FieldVerificationProvider<bool, CheckBox<TOwner>, TOwner>(this);

        protected override bool GetValue()
        {
            return Scope.Selected;
        }

        protected override void SetValue(bool value)
        {
            IWebElement element = Scope;
            if (element.Selected != value)
                element.Click();
        }

        /// <summary>
        /// Checks the control.
        /// </summary>
        /// <returns>The owner page object.</returns>
        public TOwner Check()
        {
            return Set(true);
        }

        /// <summary>
        /// Unchecks the control.
        /// </summary>
        /// <returns>The owner page object.</returns>
        public TOwner Uncheck()
        {
            return Set(false);
        }
    }
}
