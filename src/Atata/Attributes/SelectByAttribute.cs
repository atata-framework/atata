using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class SelectByAttribute : Attribute, ITermSettings
    {
        protected SelectByAttribute(TermCase termCase)
            : this(TermMatch.Inherit, termCase)
        {
        }

        protected SelectByAttribute(TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
        {
            Match = match;
            Case = termCase;
        }

        public new TermMatch Match { get; private set; }
        public TermCase Case { get; private set; }
        public string StringFormat { get; set; }
    }
}
