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
            Log.StartSettingSection(ComponentName, value);

            RunTriggersBefore();
            SetValue(value);
            RunTriggersAfter();

            Log.EndSection();

            return Owner;
        }
    }
}
