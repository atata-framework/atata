using System;

namespace Atata
{
    /// <summary>
    /// Defines the delegate to invoke on the specified event.
    /// </summary>
    public class InvokeDelegateAttribute : TriggerAttribute
    {
        public InvokeDelegateAttribute(Action actionDelegate, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority) =>
            ActionDelegate = actionDelegate.CheckNotNull(nameof(actionDelegate));

        /// <summary>
        /// Gets the action delegate.
        /// </summary>
        public Action ActionDelegate { get; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            ActionDelegate.Invoke();
    }
}
