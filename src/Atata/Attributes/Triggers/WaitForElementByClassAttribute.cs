using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByClassAttribute : WaitForElementAttribute
    {
        public WaitForElementByClassAttribute(string value, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.ClassName(value), on, priority, appliesTo)
        {
        }
    }
}
