namespace Atata
{
    /// <summary>
    /// Indicates that the Home key should be pressed on the specified event.
    /// By default occurs after the set.
    /// </summary>
    public class PressHomeAttribute : PressKeysAttribute
    {
        public PressHomeAttribute(TriggerEvents on = TriggerEvents.AfterSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(OpenQA.Selenium.Keys.Home, on, priority)
        {
        }
    }
}
