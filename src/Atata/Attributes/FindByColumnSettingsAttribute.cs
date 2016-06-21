using System;

namespace Atata
{
    public class FindByColumnSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByColumnSettingsAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : base(typeof(FindByColumnAttribute), match, format)
        {
        }

        public Type Strategy { get; set; }
    }
}
