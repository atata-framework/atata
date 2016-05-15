namespace Atata
{
    public class PressEnterAttribute : PressKeysAttribute
    {
        public PressEnterAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(OpenQA.Selenium.Keys.Enter, on, priority, appliesTo)
        {
        }
    }
}
