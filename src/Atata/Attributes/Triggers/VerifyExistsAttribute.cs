namespace Atata
{
    /// <summary>
    /// Indicates that the component should be verified whether it exists on the specified event. By default occurs upon the page object initialization.
    /// </summary>
    public class VerifyExistsAttribute : TriggerAttribute
    {
        public VerifyExistsAttribute(TriggerEvents on = TriggerEvents.OnPageObjectInit, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Component.Should.Exist();
        }
    }
}
