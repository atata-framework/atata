namespace Atata
{
    public class PressEnterAttribute : PressKeysAttribute
    {
        public PressEnterAttribute(TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(OpenQA.Selenium.Keys.Enter, on, priority, scope)
        {
        }
    }
}
