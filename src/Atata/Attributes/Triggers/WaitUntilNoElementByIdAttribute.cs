using OpenQA.Selenium;

namespace Atata
{
    public class WaitUntilNoElementByIdAttribute : WaitUntilNoElementAttribute
    {
        public WaitUntilNoElementByIdAttribute(string value, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
            : base(By.Id(value), on, priority, applyTo)
        {
        }
    }
}
