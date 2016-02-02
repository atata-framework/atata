using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    public abstract class CompositeTriggerAttribute : TriggerAttribute
    {
        private TriggerAttribute[] triggers;

        protected CompositeTriggerAttribute(TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
        }

        protected abstract IEnumerable<TriggerAttribute> CreateTriggers();

        public override void Run(TriggerContext context)
        {
            if (triggers == null)
                InitTriggers();

            foreach (TriggerAttribute trigger in triggers)
            {
                trigger.Run(context);
            }
        }

        private void InitTriggers()
        {
            triggers = CreateTriggers().ToArray();
            foreach (TriggerAttribute trigger in triggers)
            {
                trigger.On = On;
                trigger.Priority = Priority;
                trigger.AppliesTo = AppliesTo;
            }
        }
    }
}
