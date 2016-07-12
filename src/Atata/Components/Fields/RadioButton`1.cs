namespace Atata
{
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button,Option")]
    public class RadioButton<TOwner> : Field<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
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
