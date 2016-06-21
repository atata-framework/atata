using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class SelectByAttribute : Attribute, ITermSettings
    {
        protected SelectByAttribute(TermFormat format)
            : this(TermMatch.Inherit, format)
        {
        }

        protected SelectByAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
        {
            Match = match;
            Format = format;
        }

        public new TermMatch Match { get; private set; }
        public TermFormat Format { get; private set; }
        public string StringFormat { get; set; }
    }
}
