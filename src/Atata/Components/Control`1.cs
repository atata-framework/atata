namespace Atata
{
    public abstract class Control<TOwner> : UIComponent<TOwner>, IControl<TOwner>
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
    }
}
