namespace Atata
{
    [UIComponent("input[@type='text' or @type='password' or not(@type)]")]
    public class TextInput<T, TOwner> : GeneratableStringField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override T GetValue()
        {
            string value = Scope.GetValue();
            return ConvertStringToValue(value);
        }

        protected override void SetValue(T value)
        {
            string valueAsString = ConvertValueToString(value);
            Scope.FillInWith(valueAsString);
        }

        public TOwner Append(string value)
        {
            RunTriggers(TriggerEvents.BeforeSet);
            Log.StartSection("Append '{0}' to {1}", value, ComponentName);

            Scope.SendKeys(value);

            Log.EndSection();
            RunTriggers(TriggerEvents.AfterSet);

            return Owner;
        }
    }
}
