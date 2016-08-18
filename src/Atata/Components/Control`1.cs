namespace Atata
{
    public abstract class Control<TOwner> : UIComponent<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected Control()
        {
        }

        public DataProvider<bool, TOwner> IsEnabled => GetOrCreateDataProvider("enabled", GetIsEnabled);

        public new UIComponentVerificationProvider<Control<TOwner>, TOwner> Should => new UIComponentVerificationProvider<Control<TOwner>, TOwner>(this);

        protected virtual bool GetIsEnabled()
        {
            return Scope.Enabled;
        }

        public TOwner Click()
        {
            ExecuteTriggers(TriggerEvents.BeforeClick);
            Log.StartClickingSection(ComponentFullName);

            OnClick();

            Log.EndSection();
            ExecuteTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        protected virtual void OnClick()
        {
            Scope.Click();
        }

        public TOwner VerifyEnabled()
        {
            Log.StartVerificationSection("{0} is enabled", ComponentFullName);
            ATAssert.IsTrue(IsEnabled.Get(), "Expected {0} to be enabled", ComponentFullName);
            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyDisabled()
        {
            Log.StartVerificationSection("{0} is disabled", ComponentFullName);
            ATAssert.IsFalse(IsEnabled.Get(), "Expected {0} to be disabled", ComponentFullName);
            Log.EndSection();
            return Owner;
        }
    }
}
