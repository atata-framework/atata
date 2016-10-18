using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByClassAttribute : WaitForElementAttribute
    {
        public WaitForElementByClassAttribute(string value, WaitUntil until = WaitUntil.MissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.ClassName(value), until, on, priority, appliesTo)
        {
        }
    }
}
