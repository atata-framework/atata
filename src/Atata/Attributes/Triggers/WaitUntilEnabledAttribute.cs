namespace Atata
{
    /// <summary>
    /// Specifies the waiting for the control to become enabled.
    /// By default occurs before the click and before the set.
    /// </summary>
    public class WaitUntilEnabledAttribute : WaitingTriggerAttribute
    {
        public WaitUntilEnabledAttribute(TriggerEvents on = TriggerEvents.BeforeClick | TriggerEvents.BeforeSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            ((Control<TOwner>)context.Component).WaitTo.WithinSeconds(Timeout, RetryInterval).BeEnabled();
        }
    }
}
