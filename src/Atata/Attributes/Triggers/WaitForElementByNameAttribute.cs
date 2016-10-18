using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByNameAttribute : WaitForElementAttribute
    {
        public WaitForElementByNameAttribute(string value, WaitUntil until = WaitUntil.MissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.Name(value), until, on, priority, appliesTo)
        {
        }
    }
}
