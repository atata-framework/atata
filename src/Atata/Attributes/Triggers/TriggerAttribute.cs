using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class TriggerAttribute : Attribute
    {
        protected TriggerAttribute(TriggerEvent on = TriggerEvent.None, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
        {
            On = on;
            Priority = priority;
            Scope = scope;
        }

        public TriggerEvent On { get; set; }
        public TriggerPriority Priority { get; set; }
        public TriggerScope Scope { get; set; }

        public abstract void Run(TriggerContext context);
    }
}
