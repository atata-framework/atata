using System;

namespace Atata
{
    public class FindByColumnHeaderSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByColumnHeaderSettingsAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : base(typeof(FindByColumnHeaderAttribute), match, termCase)
        {
        }

        public Type Strategy { get; set; }
    }
}
