using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByNameAttribute : WaitForElementAttribute
    {
        public WaitForElementByNameAttribute(string value, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.Name(value), on, priority, appliesTo)
        {
        }
    }
}
