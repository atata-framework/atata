using OpenQA.Selenium;

namespace Atata
{
    public abstract class WaitForElementAttribute : ElementWaitAttribute
    {
        protected WaitForElementAttribute(By by, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
            : base(by, on, priority, applyTo)
        {
        }

        protected override void Wait(IWebElement scopeElement)
        {
            scopeElement.Exists(By);
        }
    }
}
