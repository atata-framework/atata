using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByIdAttribute : WaitForElementAttribute
    {
        public WaitForElementByIdAttribute(string value, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(By.Id(value), on, priority, scope)
        {
        }
    }
}
