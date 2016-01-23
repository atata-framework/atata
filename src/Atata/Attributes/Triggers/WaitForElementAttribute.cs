using OpenQA.Selenium;

namespace Atata
{
    public abstract class WaitForElementAttribute : TriggerAttribute
    {
        protected WaitForElementAttribute(By by, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(on, priority, scope)
        {
            By = by;
        }

        public By By { get; private set; }
        public ScopeSource ScopeSource { get; set; }

        public override void Run(TriggerContext context)
        {
            ScopeSource scopeSource = ScopeSource != ScopeSource.Inherit ? ScopeSource : context.Component.ScopeSource;
            IWebElement element = scopeSource.GetScopeElement(context.ParentComponent);
            element.Exists(By);
        }
    }
}
