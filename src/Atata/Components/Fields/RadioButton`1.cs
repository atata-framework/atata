namespace Atata
{
    /// <summary>
    /// Represents the radio button control (<c>&lt;input type="radio"&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button,Option", ComponentTypeName = "radio button")]
    [FindByLabel]
    public class RadioButton<TOwner> : Field<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}" /> of the checked state value.
        /// </summary>
        public ValueProvider<bool, TOwner> IsChecked =>
            CreateValueProvider("checked state", () => Value);

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> Should => new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> ExpectTo => Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> WaitTo => Should.Using<WaitingVerificationStrategy>();

        protected override bool GetValue() =>
            Scope.Selected;

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
