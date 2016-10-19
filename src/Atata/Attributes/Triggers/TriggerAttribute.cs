using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public abstract class TriggerAttribute : Attribute
    {
        protected TriggerAttribute(TriggerEvents on, TriggerPriority priority = TriggerPriority.Medium)
        {
            On = on;
            Priority = priority;
        }

        public TriggerEvents On { get; set; }

        public TriggerPriority Priority { get; set; }

        public TriggerScope AppliesTo { get; set; } = TriggerScope.Self;

        public virtual void ApplyMetadata(UIComponentMetadata metadata)
        {
        }

        public abstract void Execute<TOwner>(TriggerContext<TOwner> context)
            where TOwner : PageObject<TOwner>, IPageObject<TOwner>;
    }
}
