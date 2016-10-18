using OpenQA.Selenium;

namespace Atata
{
    public abstract class WaitForElementAttribute : WaitForAttribute
    {
        protected WaitForElementAttribute(By by, WaitUntil until, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(by, until, on, priority, appliesTo)
        {
        }

        protected override void Wait(IWebElement scopeElement, WaitUnit[] waitUnits)
        {
            foreach (WaitUnit unit in waitUnits)
            {
                By by = By.With(unit.Options);

                if (unit.Method == WaitMethod.Presence)
                    scopeElement.Exists(by);
                else
                    scopeElement.Missing(by);
            }
        }
    }
}
