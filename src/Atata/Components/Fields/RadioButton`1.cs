namespace Atata
{
    /// <summary>
    /// Represents the radio button control (&lt;input type="radio"&gt;). Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button,Option")]
    public class RadioButton<TOwner> : Field<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}" /> instance of the checked state value.
        /// </summary>
        public DataProvider<bool, TOwner> IsChecked => GetOrCreateDataProvider("checked", () => Get());

        public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> Should => new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner>(this);

        protected override bool GetValue()
        {
            return Scope.Selected;
        }

        /// <summary>
        /// Checks the control.
        /// </summary>
        /// <returns>The owner page object.</returns>
        public TOwner Check()
        {
            return Click();
        }
    }
}
