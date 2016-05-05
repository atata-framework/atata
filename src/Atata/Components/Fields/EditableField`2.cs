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
            ExecuteTriggers(TriggerEvents.BeforeSet);
            Log.StartSettingSection(ComponentFullName, ConvertValueToString(value));

            SetValue(value);

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterSet);

            return Owner;
        }

        public TOwner VerifyReadOnly()
        {
            Log.StartVerificationSection("{0} is read-only", ComponentFullName);
            ATAssert.IsTrue(IsReadOnly(), "Expected {0} to be read-only", ComponentFullName);
            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyNotReadOnly()
        {
            Log.StartVerificationSection("{0} is not read-only", ComponentFullName);
            ATAssert.IsFalse(IsReadOnly(), "Expected {0} not to be read-only", ComponentFullName);
            Log.EndSection();
            return Owner;
        }

        public virtual bool IsReadOnly()
        {
            return Scope.GetAttribute("readonly") != null;
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
