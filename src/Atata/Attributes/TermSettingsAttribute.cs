using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum)]
    public class TermSettingsAttribute : Attribute, ITermSettings
    {
        public TermSettingsAttribute(TermMatch match)
            : this(TermFormat.Inherit, match)
        {
        }

        public TermSettingsAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
        {
            Format = format;
            Match = match;
        }

        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; private set; }
    }
}
