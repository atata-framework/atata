using System;

namespace Atata
{
    /// <summary>
    /// Defines the settings to apply for the <see cref="VerifyTitleAttribute"/> trigger.
    /// </summary>
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
