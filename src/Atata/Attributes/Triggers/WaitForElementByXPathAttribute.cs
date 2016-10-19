using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByXPathAttribute : WaitForElementAttribute
    {
        public WaitForElementByXPathAttribute(string value, WaitUntil until = WaitUntil.MissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(By.XPath(value), until, on, priority)
        {
        }
    }
}
