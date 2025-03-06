namespace Atata;

public abstract class CompositeTriggerAttribute : TriggerAttribute
{
    private TriggerAttribute[] _triggers;

    protected CompositeTriggerAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        : base(on, priority)
    {
    }

    protected abstract IEnumerable<TriggerAttribute> CreateTriggers();

    protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
    {
        if (_triggers == null)
            InitTriggers();

        foreach (TriggerAttribute trigger in _triggers)
        {
            trigger.Execute(context);
        }
    }

    private void InitTriggers()
    {
        _triggers = [.. CreateTriggers()];

        foreach (TriggerAttribute trigger in _triggers)
        {
            trigger.On = On;
            trigger.Priority = Priority;

            trigger.TargetNames = TargetNames;
            trigger.TargetTypes = TargetTypes;
            trigger.TargetTags = TargetTags;
            trigger.TargetParentTypes = TargetParentTypes;

            trigger.ExcludeTargetNames = ExcludeTargetNames;
            trigger.ExcludeTargetTypes = ExcludeTargetTypes;
            trigger.ExcludeTargetTags = ExcludeTargetTags;
            trigger.ExcludeTargetParentTypes = ExcludeTargetParentTypes;
        }
    }
}
