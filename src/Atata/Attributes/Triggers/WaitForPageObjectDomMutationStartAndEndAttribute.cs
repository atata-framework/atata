namespace Atata;

/// <summary>
/// Indicates that the waiting should be performed for the page object DOM mutation to start and end.
/// </summary>
public class WaitForPageObjectDomMutationStartAndEndAttribute : TriggerAttribute
{
    public WaitForPageObjectDomMutationStartAndEndAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    /// <inheritdoc cref="WaitForDomMutationEndAttribute.ImmutableStateSeconds"/>
    public double ImmutableStateSeconds { get; set; } = -1;

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        TimeSpan immutableStateTime = WaitForDomMutationAttributeUtils.ResolveImmutableStateTime(context, ImmutableStateSeconds);

        context.Component.Session.UIComponentAccessChainScopeCache.ExecuteWithin(() =>
        {
            context.Component.Owner.Script.WaitForDomMutation();
            context.Component.Owner.Script.WaitForDomImmutableState(immutableStateTime);
        });
    }
}
