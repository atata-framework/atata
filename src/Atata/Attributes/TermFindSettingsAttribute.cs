using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class TermFindSettingsAttribute : Attribute
    {
        public TermFindSettingsAttribute(FindTermBy by, TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
            : this(by.ResolveFindAttributeType(), match, format)
        {
        }

        public TermFindSettingsAttribute(Type finderAttributeType, TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
        {
            FinderAttributeType = finderAttributeType;
            Match = match;
            Format = format;
        }

        public Type FinderAttributeType { get; private set; }
        public new TermMatch Match { get; set; }
        public TermFormat Format { get; set; }
    }
}
