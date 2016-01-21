using System;

namespace Atata
{
    public class FindByLabelSettingsAttribute : TermFindSettingsAttribute
    {
        public FindByLabelSettingsAttribute(Type strategy)
        {
            Strategy = strategy;
        }

        public FindByLabelSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit, Type strategy = null)
            : base(format, match)
        {
            Strategy = strategy;
        }

        public Type Strategy { get; set; }
    }
}
