using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementAttribute : WaitForAttribute
    {
        public WaitForElementAttribute(WaitBy waitBy, string selector, WaitUntil until = WaitUntil.MissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(until, on, priority)
        {
            WaitBy = waitBy;
            Selector = selector;
        }

        public WaitBy WaitBy { get; private set; }

        public string Selector { get; private set; }

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
                By by = WaitBy.GetBy(Selector).With(unit.Options);

                if (unit.Method == WaitMethod.Presence)
                    scopeElement.Exists(by);
                else
                    scopeElement.Missing(by);
            }
        }
    }
}
