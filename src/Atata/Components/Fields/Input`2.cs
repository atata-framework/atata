namespace Atata
{
    [ControlDefinition("input[@type!='button' and @type!='submit' and @type!='reset']")]
    public class Input<T, TOwner> : EditableField<T, TOwner>
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
            Log.StartSection("Append '{0}' to {1}", value, ComponentFullName);

            Scope.SendKeys(value);

            Log.EndSection();
            RunTriggers(TriggerEvents.AfterSet);

            return Owner;
        }
    }
}
