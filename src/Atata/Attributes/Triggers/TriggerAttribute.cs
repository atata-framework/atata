using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public abstract class TriggerAttribute : Attribute
    {
        protected TriggerAttribute(TriggerOn on = TriggerOn.Before)
        {
            On = on;
        }

        public TriggerOn On { get; set; }

        public abstract void Run(TriggerContext context);
    }
}
