using System;

namespace Atata
{
    public class FindByColumnSettingsAttribute : QualifierFindSettingsAttribute
    {
        public FindByColumnSettingsAttribute(Type strategy)
        {
            Strategy = strategy;
        }

        public FindByColumnSettingsAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit, Type strategy = null)
            : base(format, match)
        {
            Strategy = strategy;
        }

        public Type Strategy { get; set; }
    }
}
