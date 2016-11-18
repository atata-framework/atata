using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class TermSettingsAttribute : Attribute, ITermSettings, ISettingsAttribute
    {
        public TermSettingsAttribute()
        {
        }

        public TermSettingsAttribute(TermCase termCase)
        {
            Case = termCase;
        }

        public TermSettingsAttribute(TermMatch match)
        {
            Match = match;
        }

        public TermSettingsAttribute(TermMatch match, TermCase termCase)
        {
            Match = match;
            Case = termCase;
        }

        public PropertyBag Properties { get; } = new PropertyBag();

        public new TermMatch Match
        {
            get { return Properties.Get(nameof(Match), TermMatch.Equals); }
            private set { Properties[nameof(Match)] = value; }
        }

        public TermCase Case
        {
            get { return Properties.Get(nameof(Case), TermCase.None); }
            private set { Properties[nameof(Case)] = value; }
        }

        public string Format
        {
            get { return Properties.Get<string>(nameof(Format)); }
            set { Properties[nameof(Format)] = value; }
        }
    }
}
