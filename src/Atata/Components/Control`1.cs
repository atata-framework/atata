namespace Atata
{
    public abstract class Control<TOwner> : UIComponent<TOwner>, IClickable
        where TOwner : PageObject<TOwner>
    {
        protected Control()
        {
        }

        public TOwner Click()
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.StartClickingSection(ComponentFullName);

            Scope.Click();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        void IClickable.Click()
        {
            Click();
        }

        public TOwner VerifyEnabled()
        {
            Log.StartVerificationSection("{0} is enabled", ComponentFullName);
            Assert.IsTrue(IsEnabled(), "Expected {0} to be enabled", ComponentFullName);
            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyDisabled()
        {
            Log.StartVerificationSection("{0} is disabled", ComponentFullName);
            Assert.IsTrue(!IsEnabled(), "Expected {0} to be disabled", ComponentFullName);
            Log.EndSection();
            return Owner;
        }

        public virtual bool IsEnabled()
        {
            return Scope.Enabled;
        }
    }
}
