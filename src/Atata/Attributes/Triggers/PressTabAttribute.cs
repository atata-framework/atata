namespace Atata
{
    public class PressTabAttribute : PressKeysAttribute
    {
        public PressTabAttribute(TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
            : base(OpenQA.Selenium.Keys.Tab, on, priority, applyTo)
        {
        }
    }
}
