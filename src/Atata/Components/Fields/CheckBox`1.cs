using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the checkbox control (&lt;input type="checkbox"&gt;).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='checkbox']", IgnoreNameEndings = "Checkbox,CheckBox,Option")]
    public class CheckBox<TOwner> : EditableField<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
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
