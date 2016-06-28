using System;

namespace Atata
{
    public class FindByLabelSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByLabelSettingsAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(typeof(FindByLabelAttribute), match, termCase)
        {
        }

        public Type Strategy { get; set; }
    }
}
