using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByCssAttribute : WaitForElementAttribute
    {
        public WaitForElementByCssAttribute(string value, TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(By.CssSelector(value), on, priority, scope)
        {
        }
    }
}
