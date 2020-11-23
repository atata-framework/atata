namespace Atata
{
    /// <summary>
    /// Represents the editable text field control.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class EditableTextField<T, TOwner> : EditableField<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the value by executing <see cref="ValueGetBehaviorAttribute"/>.
        /// The default behavior is <see cref="ValueGetFromValueAttribute"/>.
        /// </summary>
        /// <returns>
        /// The value.
        /// </returns>
        protected override T GetValue()
        {
            var behavior = Metadata.Get<ValueGetBehaviorAttribute>()
                ?? new ValueGetFromValueAttribute();

            string valueAsString = behavior.Execute(this);
            return ConvertStringToValueUsingGetFormat(valueAsString);
        }

        /// <summary>
        /// Sets the value by executing <see cref="ValueSetBehaviorAttribute"/> behavior.
        /// The default behavior is <see cref="ValueSetUsingClearAndSendKeysAttribute"/>.
        /// If the value is null or empty, calls <see cref="OnClear()"/> method instead.
        /// </summary>
        /// <param name="value">The value.</param>
        protected override void SetValue(T value)
        {
            string valueAsString = ConvertValueToStringUsingSetFormat(value);

            if (string.IsNullOrEmpty(valueAsString))
            {
                OnClear();
            }
            else
            {
                var behavior = Metadata.Get<ValueSetBehaviorAttribute>()
                    ?? new ValueSetUsingClearAndSendKeysAttribute();

                behavior.Execute(this, valueAsString);
            }
        }

        /// <summary>
        /// Clears the value.
        /// By default uses <see cref="ValueClearUsingClearMethodAttribute"/> behavior.
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

        /// <summary>
        /// Clears the value by executing <see cref="ValueClearBehaviorAttribute"/> behavior.
        /// The default behavior is <see cref="ValueClearUsingClearMethodAttribute"/>.
        /// </summary>
        protected virtual void OnClear()
        {
            var behavior = Metadata.Get<ValueClearBehaviorAttribute>()
                ?? new ValueClearUsingClearMethodAttribute();

            behavior.Execute(this);
        }
    }
}
