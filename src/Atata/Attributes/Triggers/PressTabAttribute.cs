namespace Atata
{
    /// <summary>
    /// Indicates that the <c>Tab</c> key should be pressed on the specified event.
    /// By default occurs after the set.
    /// </summary>
    public class PressTabAttribute : PressKeysAttribute
    {
        public PressTabAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(OpenQA.Selenium.Keys.Tab, on, priority)
        {
        }
    }
}
