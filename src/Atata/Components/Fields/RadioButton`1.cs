namespace Atata
{
    /// <summary>
    /// Represents the radio button control (&lt;input type="radio"&gt;).
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button,Option")]
    public class RadioButton<TOwner> : Field<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public DataProvider<bool, TOwner> IsChecked => GetOrCreateDataProvider("checked", () => Get());

        public new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner> Should => new FieldVerificationProvider<bool, RadioButton<TOwner>, TOwner>(this);

        protected override bool GetValue()
        {
            return Scope.Selected;
        }

        public TOwner Check()
        {
            return Click();
        }
    }
}
