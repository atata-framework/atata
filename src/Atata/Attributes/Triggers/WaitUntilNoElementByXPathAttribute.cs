using OpenQA.Selenium;

namespace Atata
{
    public class WaitUntilNoElementByXPathAttribute : WaitUntilNoElementAttribute
    {
        public WaitUntilNoElementByXPathAttribute(string value, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.XPath(value), on, priority, appliesTo)
        {
        }
    }
}
