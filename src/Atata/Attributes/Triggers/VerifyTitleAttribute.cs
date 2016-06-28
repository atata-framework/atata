using System;

namespace Atata
{
    /// <summary>
    /// Indicates the verification of the page title when the page object is initialized.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class VerifyTitleAttribute : TermVerificationTriggerAttribute, ITermSettings
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
            context.Component.Owner.PageTitle.VerifyUntilMatchesAny(Match, values);
        }
    }
}
