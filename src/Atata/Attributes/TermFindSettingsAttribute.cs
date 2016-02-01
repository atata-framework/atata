using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class TermFindSettingsAttribute : Attribute
    {
        public TermFindSettingsAttribute(FindTermBy by, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
            : this(by.ResolveFindAttributeType(), format, match)
        {
        }

        public TermFindSettingsAttribute(Type finderAttributeType, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
        {
            FinderAttributeType = finderAttributeType;
            Format = format;
            Match = match;
        }

        public Type FinderAttributeType { get; private set; }
        public TermFormat Format { get; set; }
        public new TermMatch Match { get; set; }
    }
}
