using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class TermSettingsAttribute : Attribute, ITermSettings
    {
        public TermSettingsAttribute(TermCase termCase)
            : this(TermMatch.Inherit, termCase)
        {
        }

        public TermSettingsAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
        {
            Match = match;
            Case = termCase;
        }

        public new TermMatch Match { get; private set; }
        public TermCase Case { get; private set; }
        public string Format { get; set; }
    }
}
