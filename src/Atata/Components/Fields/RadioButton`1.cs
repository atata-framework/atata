namespace Atata
{
    [UIComponent("input[@type='radio']", IgnoreNameEndings = "RadioButton,Radio,Button")]
    public class RadioButton<TOwner> : Field<bool, TOwner>
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
