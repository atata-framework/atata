namespace Atata
{
    /// <summary>
    /// Specifies the information message to be logged on the defined event. By default occurs after the set.
    /// </summary>
    public class PressEnterAttribute : PressKeysAttribute
    {
        public PressEnterAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(OpenQA.Selenium.Keys.Enter, on, priority, appliesTo)
        {
        }
    }
}
