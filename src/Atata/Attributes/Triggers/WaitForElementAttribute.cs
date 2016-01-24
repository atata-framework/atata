using OpenQA.Selenium;

namespace Atata
{
    public abstract class WaitForElementAttribute : ElementWaitAttribute
    {
        protected WaitForElementAttribute(By by, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(by, on, priority, appliesTo)
        {
        }

        protected override void Wait(IWebElement scopeElement)
        {
            scopeElement.Exists(By);
        }
    }
}
