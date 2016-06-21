using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class TermSettingsAttribute : Attribute, ITermSettings
    {
        public TermSettingsAttribute(TermFormat format = TermFormat.Inherit)
            : this(TermMatch.Inherit, format)
        {
        }

        public TermSettingsAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
        {
            Format = format;
            Match = match;
        }

        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; private set; }
        public string StringFormat { get; set; }
    }
}
