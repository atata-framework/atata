using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class TriggerAttribute : Attribute
    {
        protected TriggerAttribute(TriggerEvent on = TriggerEvent.None, TriggerPriority priority = TriggerPriority.Medium)
        {
            On = on;
            Priority = priority;
        }

        public TriggerEvent On { get; set; }
        public TriggerPriority Priority { get; set; }

        public abstract void Run(TriggerContext context);
    }
}
