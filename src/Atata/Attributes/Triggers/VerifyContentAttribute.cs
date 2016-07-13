using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class VerifyContentAttribute : TriggerAttribute
    {
        public VerifyContentAttribute(params string[] values)
            : base(TriggerEvents.OnPageObjectInit)
        {
            Values = values;
        }

        public string[] Values { get; private set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Component.Content.Should.WithRetry.ContainAll(Values);
        }
    }
}
