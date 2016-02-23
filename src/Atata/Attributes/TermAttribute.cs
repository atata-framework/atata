using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TermAttribute : Attribute
    {
        public TermAttribute(TermMatch match)
            : this(null, TermFormat.Inherit, match)
        {
        }

        public TermAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : this(null, format, match)
        {
        }

        public TermAttribute(string value, TermMatch match)
            : this(new[] { value }, TermFormat.None, match: match)
        {
        }

        public TermAttribute()
            : this(null, TermFormat.Inherit)
        {
        }

        public TermAttribute(params string[] values)
            : this(values, TermFormat.None)
        {
        }

        private TermAttribute(string[] values = null, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
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
