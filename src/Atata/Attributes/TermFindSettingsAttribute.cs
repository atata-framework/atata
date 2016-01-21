using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = true)]
    public abstract class TermFindSettingsAttribute : Attribute
    {
        protected TermFindSettingsAttribute(TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
        {
            Format = format;
            Match = match;
        }

        public TermFormat Format { get; set; }
        public new TermMatch Match { get; set; }
    }
}
