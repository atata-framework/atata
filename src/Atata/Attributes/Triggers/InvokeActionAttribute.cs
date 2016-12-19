using System;

namespace Atata
{
    /// <summary>
    /// Defines the action to invoke on the specified event.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
    public class InvokeActionAttribute : TriggerAttribute
    {
        public InvokeActionAttribute(Action action, TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
            Action = action.CheckNotNull(nameof(action));
        }

        /// <summary>
        /// Gets the action delegate.
        /// </summary>
        public Action Action { get; private set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            Action();
        }
    }
}
