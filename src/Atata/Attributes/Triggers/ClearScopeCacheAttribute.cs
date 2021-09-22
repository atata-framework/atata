namespace Atata
{
    /// <summary>
    /// Indicates that the component's scope cache should be cleared on the specified event.
    /// By default occurs after click or set.
    /// </summary>
    public class ClearScopeCacheAttribute : TriggerAttribute
    {
        public ClearScopeCacheAttribute(TriggerEvents on = TriggerEvents.AfterClickOrSet, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>
        /// Gets or sets the target component whose scope cache should be cleared.
        /// The default value is <see cref="ClearScopeCacheTarget.Self"/>.
        /// </summary>
        public ClearScopeCacheTarget Of { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to clear a cache of only descendant components (excluding self component).
        /// </summary>
        public bool OnlyDescendants { get; set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            var targetComponent = GetTargetComponent(context.Component, Of);

            if (OnlyDescendants)
                targetComponent.ClearScopeCacheOfDescendants();
            else
                targetComponent.ClearScopeCache();
        }

        private static IUIComponent<TOwner> GetTargetComponent<TOwner>(IUIComponent<TOwner> component, ClearScopeCacheTarget target)
            where TOwner : PageObject<TOwner>
        {
            switch (target)
            {
                case ClearScopeCacheTarget.Self:
                    return component;
                case ClearScopeCacheTarget.Parent:
                    return component.Parent ?? throw UIComponentNotFoundException.ForParentOf(component.ComponentFullName);
                case ClearScopeCacheTarget.Grandparent:
                    return component.Parent?.Parent ?? throw UIComponentNotFoundException.ForGrandparentOf(component.ComponentFullName);
                case ClearScopeCacheTarget.GreatGrandparent:
                    return component.Parent?.Parent?.Parent ?? throw UIComponentNotFoundException.ForGreatGrandparent(component.ComponentFullName);
                case ClearScopeCacheTarget.PageObject:
                    return component.Owner;
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(target, nameof(target));
            }
        }
    }
}
