namespace Atata
{
    public abstract class Control<TOwner> : UIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected Control()
        {
        }

        public virtual bool IsEnabled
        {
            get { return Scope.Enabled; }
        }

        public ControlVerificationProvider<Control<TOwner>, TOwner> Should => new ControlVerificationProvider<Control<TOwner>, TOwner>(this);

        public TOwner Click()
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.StartClickingSection(ComponentFullName);

            Scope.Click();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        public TOwner VerifyEnabled()
        {
            Log.StartVerificationSection("{0} is enabled", ComponentFullName);
            ATAssert.IsTrue(IsEnabled, "Expected {0} to be enabled", ComponentFullName);
            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyDisabled()
        {
            Log.StartVerificationSection("{0} is disabled", ComponentFullName);
            ATAssert.IsFalse(IsEnabled, "Expected {0} to be disabled", ComponentFullName);
            Log.EndSection();
            return Owner;
        }
    }
}
