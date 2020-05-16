namespace Atata
{
    /// <summary>
    /// Indicates that the End key should be pressed on the specified event.
    /// By default occurs after the set.
    /// </summary>
    public class PressEndAttribute : PressKeysAttribute
    {
        public PressEndAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(OpenQA.Selenium.Keys.End, on, priority)
        {
        }
    }
}
