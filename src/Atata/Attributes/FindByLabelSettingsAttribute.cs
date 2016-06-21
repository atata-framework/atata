using System;

namespace Atata
{
    public class FindByLabelSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByLabelSettingsAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(typeof(FindByLabelAttribute), match, format)
        {
        }

        public Type Strategy { get; set; }
    }
}
