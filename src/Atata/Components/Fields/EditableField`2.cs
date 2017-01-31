namespace Atata
{
    /// <summary>
    /// Represents the base class for the editable field controls.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public abstract class EditableField<T, TOwner> : Field<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected EditableField()
        {
        }

        public DataProvider<bool, TOwner> IsReadOnly => GetOrCreateDataProvider("read-only", GetIsReadOnly);

        public new FieldVerificationProvider<T, EditableField<T, TOwner>, TOwner> Should => new FieldVerificationProvider<T, EditableField<T, TOwner>, TOwner>(this);

        protected virtual bool GetIsReadOnly()
        {
            return Attributes.ReadOnly;
        }

        protected abstract void SetValue(T value);

        public TOwner Set(T value)
        {
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.Start(new DataSettingLogSection(this, ConvertValueToString(value)));

            SetValue(value);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        public TOwner SetRandom()
        {
            T value = ValueRandomizer.GetRandom<T>(Metadata);
            return Set(value);
        }

        public TOwner SetRandom(out T value)
        {
            value = ValueRandomizer.GetRandom<T>(Metadata);
            return Set(value);
        }
    }
}
