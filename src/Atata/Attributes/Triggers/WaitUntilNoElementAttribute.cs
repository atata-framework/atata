using OpenQA.Selenium;

namespace Atata
{
    public abstract class WaitUntilNoElementAttribute : ElementWaitAttribute
    {
        protected WaitUntilNoElementAttribute(By by, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(by, on, priority, appliesTo)
        {
        }

        protected override void Wait(IWebElement scopeElement)
        {
            scopeElement.Missing(By);
        }
    }
}
