using System;

namespace Atata
{
    public class FindByColumnSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByColumnSettingsAttribute(Type strategy)
        {
            Strategy = strategy;
        }

        public FindByColumnSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit, Type strategy = null)
            : base(format, match)
        {
            Strategy = strategy;
        }

        public Type Strategy { get; set; }
    }
}
