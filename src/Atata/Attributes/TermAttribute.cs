using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TermAttribute : Attribute
    {
        public TermAttribute(TermFormat format = TermFormat.Inherit)
            : this(null, format: format)
        {
        }

        public TermAttribute(string value, TermMatch match)
            : this(new[] { value }, match: match)
        {
        }

        public TermAttribute(params string[] values)
            : this(values, TermFormat.Inherit)
        {
        }

        protected TermAttribute(string[] values = null, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
        {
            Values = values;
            Format = format;
            Match = match;
            CutEnding = true;
        }

        public string[] Values { get; private set; }
        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; set; }
        public bool CutEnding { get; set; }
    }
}
