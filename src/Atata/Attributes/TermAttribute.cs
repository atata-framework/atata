using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class TermAttribute : Attribute, ITermSettings
    {
        public TermAttribute(TermFormat format)
            : this(null, format: format)
        {
        }

        public TermAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : this(null, match, format)
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

        private TermAttribute(string[] values = null, TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
        {
            Values = values;
            Match = match;
            Format = format;
            CutEnding = true;
        }

        public string[] Values { get; private set; }
        public new TermMatch Match { get; set; }
        public TermFormat Format { get; private set; }
        public string StringFormat { get; set; }
        public bool CutEnding { get; set; }
    }
}
