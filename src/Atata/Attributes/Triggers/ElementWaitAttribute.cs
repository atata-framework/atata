using OpenQA.Selenium;

namespace Atata
{
    public abstract class ElementWaitAttribute : TriggerAttribute
    {
        protected ElementWaitAttribute(By by, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
            By = by;
        }

        public By By { get; private set; }
        public ScopeSource ScopeSource { get; set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            ScopeSource scopeSource = ScopeSource != ScopeSource.Inherit ? ScopeSource : context.Component.ScopeSource;
            IWebElement element = scopeSource.GetScopeElement((UIComponent)context.Component.Parent);
            Wait(element);
        }

        protected abstract void Wait(IWebElement scopeElement);
    }
}
