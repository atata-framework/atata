namespace Atata;

/// <summary>
/// Indicates that a waiting should be performed for the self or targeted component DOM mutation to end.
/// By default if <see cref="ControlName"/> is not specified, the waiting will be performed for the current component.
/// </summary>
public class WaitForDomMutationEndAttribute : TriggerAttribute
{
    public WaitForDomMutationEndAttribute(
        TriggerEvents on,
        TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    /// <summary>
    /// Gets or sets the name of the control that should be observed for DOM changes.
    /// If it is not specified, the waiting will be performed for the current component.
    /// </summary>
    public string? ControlName { get; set; }

    /// <summary>
    /// Gets or sets the value of immutable/stable DOM state seconds to wait for.
    /// If it is not specified, the immutable state time is taken from <see cref="WebSession.WaitForDomImmutableStateTime"/> property,
    /// which is 100 milliseconds by default.
    /// </summary>
    public double ImmutableStateSeconds { get; set; } = -1;

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        TimeSpan immutableStateTime = WaitForDomMutationAttributeUtils.ResolveImmutableStateTime(context, ImmutableStateSeconds);
        IUIComponent<TOwner> targetComponent = WaitForDomMutationAttributeUtils.ResolveTargetComponent(this, context, ControlName);

        targetComponent.Script.WaitForDomImmutableState(immutableStateTime);
    }
}
