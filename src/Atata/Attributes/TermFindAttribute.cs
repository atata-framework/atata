using System;
using System.Linq;

namespace Atata
{
    public abstract class TermFindAttribute : FindAttribute, ITermFindAttribute, ITermMatchFindAttribute
    {
        protected TermFindAttribute(TermMatch match)
            : this(null, TermFormat.Inherit, match)
        {
        }

        protected TermFindAttribute(TermFormat format, TermMatch match = TermMatch.Inherit)
            : this(null, format, match)
        {
        }

        protected TermFindAttribute(string value, TermMatch match)
            : this(new[] { value }, match: match)
        {
        }

        protected TermFindAttribute(params string[] values)
            : this(values, TermFormat.Inherit)
        {
        }

        private TermFindAttribute(string[] values = null, TermFormat format = TermFormat.Inherit, TermMatch match = TermMatch.Inherit)
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

        protected abstract TermFormat DefaultFormat { get; }
        protected abstract TermMatch DefaultMatch { get; }

        public virtual string[] GetTerms(UIComponentMetadata metadata)
        {
            if (Values != null && Values.Any())
            {
                return Values;
            }
            else
            {
                TermAttribute termAttribute = metadata.GetTerm(x => x.Values != null && x.Values.Any());
                if (termAttribute != null)
                    return termAttribute.Values;
            }
            return new[] { GetTermFromProperty(metadata) };
        }

        private string GetTermFromProperty(UIComponentMetadata metadata)
        {
            TermFormat format = GetTermFormat(metadata);
            string name = GetPropertyName(metadata);
            return format.ApplyTo(name);
        }

        private TermFormat GetTermFormat(UIComponentMetadata metadata)
        {
            TermAttribute termAttribute;
            if (Format != TermFormat.Inherit)
                return Format;
            else if ((termAttribute = metadata.GetTerm(x => x.Format != TermFormat.Inherit)) != null)
                return termAttribute.Format;
            else
                return GetTermFormatFromMetadata(metadata);
        }

        public TermMatch GetTermMatch(UIComponentMetadata metadata)
        {
            TermAttribute termAttribute;
            if (Match != TermMatch.Inherit)
                return Match;
            else if ((termAttribute = metadata.GetTerm(x => x.Match != TermMatch.Inherit)) != null)
                return termAttribute.Match;
            else
                return GetTermMatchFromMetadata(metadata);
        }

        private string GetPropertyName(UIComponentMetadata metadata)
        {
            string name = metadata.Name;
            TermAttribute termAttribute = metadata.GetTerm();

            if (CutEnding && (termAttribute == null || termAttribute.CutEnding) && metadata.ComponentDefinitonAttribute != null)
            {
                string suffixToIgnore = metadata.ComponentDefinitonAttribute.GetIgnoreNameEndingValues().
                    FirstOrDefault(x => name.EndsWith(x) && name.Length > x.Length);

                if (suffixToIgnore != null)
                    return name.Substring(0, name.Length - suffixToIgnore.Length).TrimEnd();
            }
            return name;
        }

        private TermFormat GetTermFormatFromMetadata(UIComponentMetadata metadata)
        {
            Type thisType = GetType();
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<TermFindSettingsAttribute>(x => x.FinderAttributeType == thisType && x.Format != TermFormat.Inherit);
            return settingsAttribute != null ? settingsAttribute.Format : DefaultFormat;
        }

        private TermMatch GetTermMatchFromMetadata(UIComponentMetadata metadata)
        {
            Type thisType = GetType();
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<TermFindSettingsAttribute>(x => x.FinderAttributeType == thisType && x.Match != TermMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
