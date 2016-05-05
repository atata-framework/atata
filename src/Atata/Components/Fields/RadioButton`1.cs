namespace Atata
{
    [ControlDefinition("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button,Option")]
    public class RadioButton<TOwner> : Field<bool, TOwner>, ICheckable<TOwner>
        where TOwner : PageObject<TOwner>
    {
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
