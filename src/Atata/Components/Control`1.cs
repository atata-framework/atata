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
            Log.StartClickingSection(ComponentName);
            RunTriggersBefore();

            Scope.Click();

            RunTriggersAfter();
            Log.EndSection();

            return Owner;
        }

        void IClickable.Click()
        {
            Click();
        }
    }
}
