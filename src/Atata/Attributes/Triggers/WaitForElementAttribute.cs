using OpenQA.Selenium;

namespace Atata
{
    public abstract class WaitForElementAttribute : WaitForAttribute
    {
        protected WaitForElementAttribute(By by, WaitUntil until, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(by, until, on, priority)
        {
            By = by;
        }

        public By By { get; private set; }

        public ScopeSource ScopeSource { get; set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            ScopeSource scopeSource = ScopeSource != ScopeSource.Inherit ? ScopeSource : context.Component.ScopeSource;
            IWebElement scopeElement = scopeSource.GetScopeElement((UIComponent)context.Component.Parent);

            WaitUnit[] waitUnits = GetWaitUnits(Until);

            Wait(scopeElement, waitUnits);
        }

        protected override void Wait(IWebElement scopeElement, WaitUnit[] waitUnits)
        {
            foreach (WaitUnit unit in waitUnits)
            {
                By by = By.With(unit.Options);

                if (unit.Method == WaitMethod.Presence)
                    scopeElement.Exists(by);
                else
                    scopeElement.Missing(by);
            }
        }
    }
}
