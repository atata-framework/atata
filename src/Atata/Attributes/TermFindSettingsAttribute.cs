using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = true)]
    public class TermFindSettingsAttribute : Attribute
    {
        public TermFindSettingsAttribute(FindTermBy by, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
            : this(by.ResolveFindAttributeType(), match, termCase)
        {
        }

        public TermFindSettingsAttribute(Type finderAttributeType, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
        {
            FinderAttributeType = finderAttributeType;
            Match = match;
            Case = termCase;
        }

        public Type FinderAttributeType { get; private set; }
        public new TermMatch Match { get; set; }
        public TermCase Case { get; set; }
    }
}
