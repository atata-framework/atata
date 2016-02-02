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
            RunTriggers(TriggerEvents.BeforeClick);
            Log.StartClickingSection(ComponentName);

            Scope.Click();

            Log.EndSection();
            RunTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        void IClickable.Click()
        {
            Click();
        }
    }
}
