namespace Atata
{
    public class PressEnterAttribute : PressKeysAttribute
    {
        public PressEnterAttribute(TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
            : base(OpenQA.Selenium.Keys.Enter, on, priority, applyTo)
        {
        }
    }
}
