using System;

namespace Atata
{
    public class FindByColumnHeaderSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByColumnHeaderSettingsAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(typeof(FindByColumnHeaderAttribute), match, format)
        {
        }

        public Type Strategy { get; set; }
    }
}
