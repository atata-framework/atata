using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByIdAttribute : WaitForElementAttribute
    {
        public WaitForElementByIdAttribute(string value, WaitUntil until = WaitUntil.MissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(By.Id(value), until, on, priority)
        {
        }
    }
}
