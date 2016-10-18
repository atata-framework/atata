using OpenQA.Selenium;

namespace Atata
{
    public class WaitForElementByCssAttribute : WaitForElementAttribute
    {
        public WaitForElementByCssAttribute(string value, WaitUntil until = WaitUntil.MissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(By.CssSelector(value), until, on, priority, appliesTo)
        {
        }
    }
}
