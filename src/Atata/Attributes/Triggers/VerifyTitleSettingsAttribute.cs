using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class VerifyTitleSettingsAttribute : TermSettingsAttribute
    {
        public VerifyTitleSettingsAttribute(TermCase termCase = TermCase.Inherit)
            : base(termCase)
        {
        }

        public VerifyTitleSettingsAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : base(match, termCase)
        {
        }
    }
}
