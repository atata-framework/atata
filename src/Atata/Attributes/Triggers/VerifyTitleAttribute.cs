using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Specifies the verification of the page title.
    /// By default occurs upon the page object initialization.
    /// If no value is specified, it uses the class name as the expected value with the <see cref="TermCase.Title"/> casing applied.
    /// </summary>
    public class VerifyTitleAttribute : TermVerificationTriggerAttribute
    {
        public VerifyTitleAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public VerifyTitleAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        public VerifyTitleAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        public VerifyTitleAttribute(params string[] values)
            : base(values)
        {
        }

        protected override IEnumerable<IHasOptionalProperties> GetSettingsAttributes(UIComponentMetadata metadata) =>
            metadata.GetAll<VerifyTitleSettingsAttribute>();

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            var metadata = context.Component.Metadata;

            context.Component.Owner.PageTitle.Should
                .WithinSeconds(Timeout, RetryInterval)
                .MatchAny(ResolveMatch(metadata), values);
        }
    }
}
