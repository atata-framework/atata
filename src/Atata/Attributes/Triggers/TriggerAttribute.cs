using System;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the triggers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class TriggerAttribute : Attribute
    {
        protected TriggerAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        {
            On = on;
            Priority = priority;
        }

        /// <summary>
        /// Gets or sets the trigger events.
        /// </summary>
        public TriggerEvents On { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// The default value is <see cref="TriggerPriority.Medium"/>.
        /// </summary>
        public TriggerPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the scope to apply the trigger to.
        /// The default value is <see cref="TriggerScope.Self"/>.
        /// </summary>
        public TriggerScope AppliesTo { get; set; } = TriggerScope.Self;

        /// <summary>
        /// Gets a value indicating whether this trigger is defined at the component level.
        /// </summary>
        public bool IsDefinedAtComponentLevel { get; internal set; }

        protected internal virtual void ApplyMetadata(UIComponentMetadata metadata)
        {
        }

        /// <summary>
        /// Executes the specified trigger action.
        /// </summary>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="context">The trigger context.</param>
        protected internal abstract void Execute<TOwner>(TriggerContext<TOwner> context)
            where TOwner : PageObject<TOwner>, IPageObject<TOwner>;
    }
}
