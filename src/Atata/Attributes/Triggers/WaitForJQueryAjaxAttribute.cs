using System;

namespace Atata
{
    /// <summary>
    /// Indicates that the waiting should be performed until the jQuery AJAX is completed. By default occurs after the click.
    /// </summary>
    public class WaitForJQueryAjaxAttribute : TriggerAttribute
    {
        public WaitForJQueryAjaxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            bool completed = context.Driver.Try().Until(
                x => (bool)context.Driver.ExecuteScript("return jQuery.active == 0"));

            if (!completed)
                throw new TimeoutException("Timed out waiting for the jQuery AJAX call to complete.");
        }
    }
}
