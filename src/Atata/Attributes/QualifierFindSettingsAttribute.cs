using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public abstract class QualifierFindSettingsAttribute : Attribute
    {
        protected QualifierFindSettingsAttribute(QualifierFormat format = QualifierFormat.Inherit, QualifierMatch match = QualifierMatch.Inherit)
        {
            Format = format;
            Match = match;
        }

        public QualifierFormat Format { get; set; }
        public new QualifierMatch Match { get; set; }
    }
}
