using System;

namespace Atata
{
    /// <summary>
    /// Specifies the verification of the page title. By default occurs upon the page object initialization. If no value is specified, it uses the class name as the expected value with the <c>TermCase.Title</c> casing applied.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class VerifyTitleAttribute : TermVerificationTriggerAttribute
    {
        public VerifyTitleAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public VerifyTitleAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
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

        protected override ITermSettings ResolveTermSettings(UIComponentMetadata metadata)
        {
            return metadata.GetFirstOrDefaultAssemblyAttribute<VerifyTitleSettingsAttribute>();
        }

        protected override void OnExecute<TOwner>(TriggerContext<TOwner> context, string[] values)
        {
            context.Component.Owner.PageTitle.Should.WithRetry.MatchAny(Match, values);
        }
    }
}
