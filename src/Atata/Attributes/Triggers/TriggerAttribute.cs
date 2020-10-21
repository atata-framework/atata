using System;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the triggers.
    /// </summary>
    public abstract class TriggerAttribute : MulticastAttribute
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
        [Obsolete("Use Target* properties instead. For example, 'TargetAllChildren = true' is an equivalent of 'AppliesTo = TriggerScope.Children'.")] // Obsolete since v1.8.0.
        public TriggerScope AppliesTo
        {
            get => IsTargetSpecified ? TriggerScope.Children : TriggerScope.Self;
            set => TargetAnyType = value == TriggerScope.Children;
        }

        /// <summary>
        /// Gets a value indicating whether this trigger is defined at the component level.
        /// </summary>
        [Obsolete("There is no more need to use this property. " +
            "To detect whether trigger is located at component level: component.Metadata.ComponentAttributes.Contains(attribute)")] // Obsolete since v1.8.0.
        public bool IsDefinedAtComponentLevel { get; internal set; }

        [Obsolete("Do not use this method. Metadata can be accessed in Execute method by: context.Component.Metadata.")] // Obsolete since v1.8.0.
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
