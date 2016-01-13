namespace Atata
{
    [UIComponent("input[@type='text' or @type='password' or not(@type)]")]
    public class TextInput<TOwner> : GeneratableStringField<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override string GetValue()
        {
            return Scope.GetValue();
        }

        protected override void SetValue(string value)
        {
            Scope.FillInWith(value);
        }

        public TOwner Append(string value)
        {
            Log.StartSection("Append '{0}' to {1}", value, ComponentName);
            Scope.SendKeys(value);
            return Owner;
        }
    }
}
