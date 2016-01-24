using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class TriggerAttribute : Attribute
    {
        protected TriggerAttribute(TriggerEvent on, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
        {
            On = on;
            Priority = priority;
            ApplyTo = applyTo;
        }

        public TriggerEvent On { get; private set; }
        public TriggerPriority Priority { get; set; }
        public TriggerScope ApplyTo { get; set; }

        public abstract void Run(TriggerContext context);
    }
}
