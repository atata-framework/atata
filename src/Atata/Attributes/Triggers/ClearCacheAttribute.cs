namespace Atata
{
    /// <summary>
    /// Indicates that the component's cache should be cleared on the specified event.
    /// By default occurs after click or set.
    /// </summary>
    public class ClearCacheAttribute : TriggerAttribute
    {
        public ClearCacheAttribute(TriggerEvents on = TriggerEvents.AfterClickOrSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>
        /// Gets or sets the target component whose cache should be cleared.
        /// The default value is <see cref="ClearCacheTarget.Self"/>.
        /// </summary>
        public ClearCacheTarget Of { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clear a cache of only descendant components (excluding self component).
        /// </summary>
        public bool OnlyDescendants { get; set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            var targetComponent = GetTargetComponent(context.Component, Of);

            if (OnlyDescendants)
                targetComponent.ClearCacheOfDescendants();
            else
                targetComponent.ClearCache();
        }

        private static IUIComponent<TOwner> GetTargetComponent<TOwner>(IUIComponent<TOwner> component, ClearCacheTarget target)
            where TOwner : PageObject<TOwner>
        {
            switch (target)
            {
                case ClearCacheTarget.Self:
                    return component;
                case ClearCacheTarget.Parent:
                    return component.Parent ?? throw UIComponentNotFoundException.ForParentOf(component.ComponentFullName);
                case ClearCacheTarget.Grandparent:
                    return component.Parent?.Parent ?? throw UIComponentNotFoundException.ForGrandparentOf(component.ComponentFullName);
                case ClearCacheTarget.GreatGrandparent:
                    return component.Parent?.Parent?.Parent ?? throw UIComponentNotFoundException.ForGreatGrandparent(component.ComponentFullName);
                case ClearCacheTarget.PageObject:
                    return component.Owner;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(target, nameof(target));
            }
        }
    }
}
