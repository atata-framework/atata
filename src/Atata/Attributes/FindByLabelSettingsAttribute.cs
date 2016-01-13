using System;

namespace Atata
{
    public class FindByLabelSettingsAttribute : QualifierFindSettingsAttribute
    {
        public FindByLabelSettingsAttribute(Type strategy)
        {
            Strategy = strategy;
        }

        public FindByLabelSettingsAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit, Type strategy = null)
            : base(format, match)
        {
            Strategy = strategy;
        }

        public Type Strategy { get; set; }
    }
}
