namespace Atata
{
    /// <summary>
    /// Indicates that the waiting should be performed until the Angular (v2+) AJAX is completed.
    /// By default occurs after the click.
    /// </summary>
    public class WaitForAngularAttribute : TriggerAttribute
    {
        public WaitForAngularAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        /// <summary>
        /// Gets or sets the Angular root selector.
        /// The default value is taken from <see cref="AngularSettings.RootSelector"/>,
        /// which is <c>"[ng-app]"</c> by default.
        /// </summary>
        public string RootSelector { get; set; } = AngularSettings.RootSelector;

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            context.Component.Script.WaitForAngular(RootSelector);
    }
}
