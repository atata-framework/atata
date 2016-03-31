using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class VerifyTitleSettingsAttribute : TermSettingsAttribute
    {
        public VerifyTitleSettingsAttribute(TermMatch match = TermMatch.Inherit)
            : this(TermFormat.Inherit, match)
        {
        }

        public VerifyTitleSettingsAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : base(format, match)
        {
        }
    }
}
