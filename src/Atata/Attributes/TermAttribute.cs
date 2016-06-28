using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TermAttribute : Attribute, ITermSettings
    {
        public TermAttribute(TermCase termCase)
            : this(null, termCase: termCase)
        {
        }

        public TermAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : this(null, match, termCase)
        {
        }

        public TermAttribute(TermMatch match, params string[] values)
            : this(values, match)
        {
        }

        public TermAttribute(params string[] values)
            : this(values, TermMatch.Inherit)
        {
        }

        private TermAttribute(string[] values = null, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
        {
            Values = values;
            Match = match;
            Case = termCase;
            CutEnding = true;
        }

        public string[] Values { get; private set; }
        public new TermMatch Match { get; set; }
        public TermCase Case { get; private set; }
        public string Format { get; set; }
        public bool CutEnding { get; set; }
    }
}
