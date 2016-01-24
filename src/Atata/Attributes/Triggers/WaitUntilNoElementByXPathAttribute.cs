using OpenQA.Selenium;

namespace Atata
{
    public class WaitUntilNoElementByXPathAttribute : WaitUntilNoElementAttribute
    {
        public WaitUntilNoElementByXPathAttribute(string value, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(By.XPath(value), on, priority, scope)
        {
        }
    }
}
