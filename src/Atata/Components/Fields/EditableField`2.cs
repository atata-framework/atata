namespace Atata
{
    public abstract class EditableField<T, TOwner> : Field<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected EditableField()
        {
        }

        protected abstract void SetValue(T value);

        public TOwner Set(T value)
        {
            RunTriggers(TriggerEvent.BeforeSet);
            Log.StartSettingSection(ComponentName, value);

            SetValue(value);

            Log.EndSection();
            RunTriggers(TriggerEvent.AfterSet);

            return Owner;
        }
    }
}
