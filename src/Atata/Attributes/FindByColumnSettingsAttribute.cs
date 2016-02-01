using System;

namespace Atata
{
    public class FindByColumnSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByColumnSettingsAttribute(Type strategy)
            : base(typeof(FindByColumnAttribute))
        {
            Strategy = strategy;
        }

        public FindByColumnSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : base(typeof(FindByColumnAttribute), format, match)
        {
        }

        public Type Strategy { get; set; }
    }
}
