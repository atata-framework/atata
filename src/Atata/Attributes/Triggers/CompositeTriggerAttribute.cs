using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public abstract class CompositeTriggerAttribute : TriggerAttribute
    {
        private TriggerAttribute[] triggers;

        protected CompositeTriggerAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected abstract IEnumerable<TriggerAttribute> CreateTriggers();

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            if (triggers == null)
                InitTriggers();

            foreach (TriggerAttribute trigger in triggers)
            {
                trigger.Execute(context);
            }
        }

        private void InitTriggers()
        {
            triggers = CreateTriggers().ToArray();

            foreach (TriggerAttribute trigger in triggers)
            {
                trigger.On = On;
                trigger.Priority = Priority;
                trigger.TargetNames = TargetNames;
                trigger.TargetTypes = TargetTypes;
                trigger.TargetParentTypes = TargetParentTypes;
            }
        }
    }
}
