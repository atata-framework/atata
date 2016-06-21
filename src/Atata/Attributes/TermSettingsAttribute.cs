using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class TermSettingsAttribute : Attribute, ITermSettings
    {
        public TermSettingsAttribute(TermFormat format)
            : this(TermMatch.Inherit, format)
        {
        }

        public TermSettingsAttribute(TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
        {
            Match = match;
            Format = format;
        }

        public new TermMatch Match { get; private set; }
        public TermFormat Format { get; private set; }
        public string StringFormat { get; set; }
    }
}
