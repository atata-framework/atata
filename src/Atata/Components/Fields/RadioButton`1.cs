namespace Atata
{
    /// <summary>
    /// Represents the radio button control (<c>&lt;input type="radio"&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button,Option")]
    [ControlFinding(FindTermBy.Label)]
    public class RadioButton<TOwner> : Field<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}" /> instance of the checked state value.
        /// </summary>
        public DataProvider<bool, TOwner> IsChecked => GetOrCreateDataProvider("checked state", () => Value);

        /// <summary>
        /// Gets the verification provider that gives a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> Should => new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner>(this);

        protected override bool GetValue()
        {
            return Scope.Selected;
        }

        /// <summary>
        /// Checks the control.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <returns>The owner page object.</returns>
        public TOwner Check()
        {
            OnCheck();

            return Owner;
        }

        protected virtual void OnCheck()
        {
            if (!IsChecked)
                Click();
        }
    }
}
