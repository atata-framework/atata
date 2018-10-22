namespace Atata
{
    /// <summary>
    /// Indicates that the <c>Enter</c> key should be pressed on the specified event.
    /// By default occurs after the set.
    /// </summary>
    public class PressEnterAttribute : PressKeysAttribute
    {
        public PressEnterAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(OpenQA.Selenium.Keys.Enter, on, priority)
        {
        }
    }
}
