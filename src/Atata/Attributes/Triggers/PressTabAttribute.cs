namespace Atata
{
    public class PressTabAttribute : PressKeysAttribute
    {
        public PressTabAttribute(TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(OpenQA.Selenium.Keys.Tab, on, priority, scope)
        {
        }
    }
}
