using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class SelectByAttribute : Attribute, ITermSettings
    {
        protected SelectByAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
        {
            Format = format;
            Match = match;
        }

        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; private set; }
        public string StringFormat { get; set; }
    }
}
