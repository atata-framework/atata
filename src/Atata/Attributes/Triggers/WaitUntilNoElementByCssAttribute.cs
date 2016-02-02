using OpenQA.Selenium;

namespace Atata
{
    public class WaitUntilNoElementByCssAttribute : WaitUntilNoElementAttribute
    {
        public WaitUntilNoElementByCssAttribute(string value, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.CssSelector(value), on, priority, appliesTo)
        {
        }
    }
}
