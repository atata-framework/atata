namespace Atata
{
    /// <summary>
    /// Indicates that the Escape key should be pressed on the specified event.
    /// By default occurs after the set.
    /// </summary>
    public class PressEscapeAttribute : PressKeysAttribute
    {
        public PressEscapeAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(OpenQA.Selenium.Keys.Escape, on, priority)
        {
        }
    }
}
