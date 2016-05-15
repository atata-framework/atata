using System;

namespace Atata
{
    public class WaitUntilJQueryAjaxAttribute : TriggerAttribute
    {
        public WaitUntilJQueryAjaxAttribute(TriggerEvents on = TriggerEvents.AfterClickOrSet, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            bool completed = context.Driver.Try().Until(
                x => (bool)context.Driver.ExecuteScript("return jQuery.active == 0"));

            if (!completed)
                throw new Exception("Timed out waiting for jQuery AJAX call to complete.");
        }
    }
}
