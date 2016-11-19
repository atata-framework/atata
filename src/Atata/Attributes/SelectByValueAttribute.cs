using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectByValueAttribute : TermSettingsAttribute
    {
        public SelectByValueAttribute()
        {
        }

        public SelectByValueAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByValueAttribute(TermMatch match)
            : base(match)
        {
        }

        public SelectByValueAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }
    }
}
