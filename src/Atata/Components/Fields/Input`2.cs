namespace Atata
{
    /// <summary>
    /// Represents the input control.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
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

        /// <summary>
        /// Appends the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The owner page object.</returns>
        public TOwner Append(string value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataAdditionLogSection(this, value) { ActionText = "Append" });

            Scope.SendKeys(value);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        public TOwner Clear()
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataClearingLogSection(this));

            OnClear();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        protected virtual void OnClear()
        {
            Scope.Clear();
        }
    }
}
