namespace Atata;

/// <summary>
/// Indicates that the waiting should be performed for the page object DOM mutation to end.
/// </summary>
public class WaitForPageObjectDomMutationEndAttribute : TriggerAttribute
{
    public WaitForPageObjectDomMutationEndAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    /// <inheritdoc cref="WaitForDomMutationEndAttribute.ImmutableStateSeconds"/>
    public double ImmutableStateSeconds { get; set; } = -1;

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        TimeSpan immutableStateTime = WaitForDomMutationAttributeUtils.ResolveImmutableStateTime(ImmutableStateSeconds);

        context.Component.Owner.Script.WaitForDomImmutableState(immutableStateTime);
    }
}
