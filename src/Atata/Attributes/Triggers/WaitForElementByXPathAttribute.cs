using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByXPathAttribute : WaitForElementAttribute
    {
        public WaitForElementByXPathAttribute(string value, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.XPath(value), on, priority, appliesTo)
        {
        }
    }
}
