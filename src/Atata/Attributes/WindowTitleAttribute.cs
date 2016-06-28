using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class WindowTitleAttribute : Attribute, ITermDataProvider
    {
        private const TermCase DefaultCase = TermCase.Title;
        private const TermMatch DefaultMatch = TermMatch.Equals;

        public WindowTitleAttribute(TermCase termCase)
            : this(null, termCase: termCase)
        {
        }

        public WindowTitleAttribute(TermMatch match, TermCase termCase = DefaultCase)
            : this(null, match, termCase)
        {
        }

        public WindowTitleAttribute(TermMatch match, params string[] values)
            : this(values, match)
        {
        }

        public WindowTitleAttribute(params string[] values)
            : this(values, DefaultMatch)
        {
        }

        private WindowTitleAttribute(string[] values = null, TermMatch match = DefaultMatch, TermCase termCase = DefaultCase)
        {
            Values = values;
            Match = match;
            Case = termCase;
        }

        public string[] Values { get; private set; }
        public TermCase Case { get; private set; }
        public new TermMatch Match { get; private set; }
        public string Format { get; set; }
    }
}
