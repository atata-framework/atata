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
            RunTriggers(TriggerEvents.BeforeSet);
            Log.StartSettingSection(ComponentName, ConvertValueToString(value));

            SetValue(value);

            Log.EndSection();
            RunTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        public TOwner VerifyReadOnly()
        {
            Log.StartVerificationSection("{0} component is read-only", ComponentName);
            Assert.That(IsReadOnly(), "Expected {0} component to be read-only", ComponentName);
            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyNotReadOnly()
        {
            Log.StartVerificationSection("{0} component is not read-only", ComponentName);
            Assert.That(!IsReadOnly(), "Expected {0} component not to be read-only", ComponentName);
            Log.EndSection();
            return Owner;
        }

        public virtual bool IsReadOnly()
        {
            return Scope.GetAttribute("readonly") != null;
        }
    }
}
