namespace Atata
{
    public class PressTabAttribute : PressKeysAttribute
    {
        public PressTabAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(OpenQA.Selenium.Keys.Tab, on, priority, appliesTo)
        {
        }
    }
}
