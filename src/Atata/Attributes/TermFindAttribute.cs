using System;
using System.Linq;

namespace Atata
{
    public abstract class TermFindAttribute : FindAttribute, ITermFindAttribute, ITermMatchFindAttribute
    {
        protected TermFindAttribute(TermFormat format)
            : this(null, format: format)
        {
        }

        protected TermFindAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : this(null, match, format)
        {
        }

        protected TermFindAttribute(TermMatch match, params string[] values)
            : this(values, match)
        {
        }

        protected TermFindAttribute(params string[] values)
            : this(values, TermMatch.Inherit)
        {
        }

        protected TermFindAttribute(string[] values = null, TermMatch match = TermMatch.Inherit, TermFormat format = TermFormat.Inherit)
        {
            Values = values;
            Match = match;
            Format = format;
            CutEnding = true;
        }

        public string[] Values { get; private set; }
        public TermFormat Format { get; private set; }
        public new TermMatch Match { get; set; }
        public bool CutEnding { get; set; }

        protected abstract TermFormat DefaultFormat { get; }

        protected virtual TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
        }

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

            if (CutEnding && (termAttribute == null || termAttribute.CutEnding))
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
