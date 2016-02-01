using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public class TermFindSettingsAttribute : Attribute
    {
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
