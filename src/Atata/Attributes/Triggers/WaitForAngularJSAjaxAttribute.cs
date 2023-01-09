using System;

namespace Atata
{
    [Obsolete("Use " + nameof(WaitForAngularJSAttribute) + " instead.")] // Obsolete since v2.6.0.
    public class WaitForAngularJSAjaxAttribute : WaitForAngularJSAttribute
    {
        public WaitForAngularJSAjaxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }
    }
}
