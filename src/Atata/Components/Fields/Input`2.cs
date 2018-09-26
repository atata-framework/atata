using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the input control (&lt;input&gt;).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type!='button' and @type!='submit' and @type!='reset']")]
    [ControlFinding(FindTermBy.Label)]
    public class Input<T, TOwner> : EditableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected override T GetValue()
        {
            string valueAsString = Attributes.Value;
            return ConvertStringToValue(valueAsString);
        }

        protected override void SetValue(T value)
        {
            string valueAsString = ConvertValueToString(value);
            Scope.FillInWith(valueAsString);
        }

        /// <summary>
        /// Appends the specified value.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The owner page object.</returns>
        public TOwner Append(string value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataAdditionLogSection(this, value) { ActionText = "Append" });

            Scope.SendKeys(Keys.End + value);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        /// <summary>
        /// Clears the value.
        /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
        /// </summary>
        /// <returns>The owner page object.</returns>
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
