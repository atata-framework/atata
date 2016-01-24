using OpenQA.Selenium;

namespace Atata
{
    public abstract class ElementWaitAttribute : TriggerAttribute
    {
        protected ElementWaitAttribute(By by, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
            By = by;
        }

        public By By { get; private set; }
        public ScopeSource ScopeSource { get; set; }

        public override void Run(TriggerContext context)
        {
            ScopeSource scopeSource = ScopeSource != ScopeSource.Inherit ? ScopeSource : context.Component.ScopeSource;
            IWebElement element = scopeSource.GetScopeElement(context.ParentComponent);
            Wait(element);
        }

        protected abstract void Wait(IWebElement scopeElement);
    }
}
