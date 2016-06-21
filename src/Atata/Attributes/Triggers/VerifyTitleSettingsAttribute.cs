using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class VerifyTitleSettingsAttribute : TermSettingsAttribute
    {
        public VerifyTitleSettingsAttribute(TermFormat format = TermFormat.Inherit)
            : base(format)
        {
        }

        public VerifyTitleSettingsAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }
    }
}
