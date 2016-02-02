using OpenQA.Selenium;

namespace Atata
{
    public class WaitUntilNoElementByIdAttribute : WaitUntilNoElementAttribute
    {
        public WaitUntilNoElementByIdAttribute(string value, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.Id(value), on, priority, appliesTo)
        {
        }
    }
}
