using System;

namespace Atata
{
    /// <summary>
    /// Specifies the verification of the page content. Verifies whether the component contains the specified content values. By default occurs during the page object initialization.
    /// </summary>
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
