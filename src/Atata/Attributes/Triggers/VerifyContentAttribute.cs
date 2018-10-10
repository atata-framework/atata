using System;

namespace Atata
{
    /// <summary>
    /// Specifies the verification of the page content.
    /// Verifies whether the component contains the specified content values.
    /// By default occurs upon the page object initialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class VerifyContentAttribute : TriggerAttribute
    {
        public VerifyContentAttribute(params string[] values)
            : base(TriggerEvents.Init)
        {
            Values = values;
        }

        public string[] Values { get; private set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Component.Content.Should.WithRetry.ContainAll(Values);
        }
    }
}
