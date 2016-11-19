using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SelectByTextAttribute : TermSettingsAttribute
    {
        public SelectByTextAttribute()
        {
        }

        public SelectByTextAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        public SelectByTextAttribute(TermMatch match)
            : base(match)
        {
        }

        public SelectByTextAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }
    }
}
