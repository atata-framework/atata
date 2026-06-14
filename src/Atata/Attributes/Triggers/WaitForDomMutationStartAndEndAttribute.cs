namespace Atata;

/// <summary>
/// Indicates that a waiting should be performed for the self or targeted component DOM mutation to start and end.
/// By default if <see cref="ControlName"/> is not specified, the waiting will be performed for the current component.
/// </summary>
public class WaitForDomMutationStartAndEndAttribute : TriggerAttribute
{
    public WaitForDomMutationStartAndEndAttribute(
        TriggerEvents on,
        TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    /// <inheritdoc cref="WaitForDomMutationEndAttribute.ControlName"/>
    public string? ControlName { get; set; }

    /// <inheritdoc cref="WaitForDomMutationEndAttribute.ImmutableStateSeconds"/>
    public double ImmutableStateSeconds { get; set; } = -1;

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        TimeSpan immutableStateTime = WaitForDomMutationAttributeUtils.ResolveImmutableStateTime(context, ImmutableStateSeconds);
        IUIComponent<TOwner> targetComponent = WaitForDomMutationAttributeUtils.ResolveTargetComponent(this, context, ControlName);

        context.Component.Session.UIComponentAccessChainScopeCache.ExecuteWithin(() =>
        {
            targetComponent.Script.WaitForDomMutation();
            targetComponent.Script.WaitForDomImmutableState(immutableStateTime);
        });
    }
}
