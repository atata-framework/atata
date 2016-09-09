using System;
using System.Linq;

namespace Atata
{
    public abstract class TermFindAttribute : FindAttribute, ITermFindAttribute, ITermMatchFindAttribute, ITermSettings
    {
        protected TermFindAttribute(TermCase termCase)
            : this(null, termCase: termCase)
        {
        }

        protected TermFindAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
            : this(null, match, termCase)
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

        protected TermFindAttribute(string[] values = null, TermMatch match = TermMatch.Inherit, TermCase termCase = TermCase.Inherit)
        {
            Values = values;
            Match = match;
            Case = termCase;
            CutEnding = true;
        }

        public string[] Values { get; private set; }
        public TermCase Case { get; private set; }
        public new TermMatch Match { get; set; }
        public string Format { get; set; }
        public bool CutEnding { get; set; }

        protected abstract TermCase DefaultCase { get; }

        protected virtual TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
        }

        public string[] GetTerms(UIComponentMetadata metadata)
        {
            string[] rawTerms = GetRawTerms(metadata);
            string format = GetTermFormat(metadata);

            return !string.IsNullOrEmpty(format) ? rawTerms.Select(x => string.Format(format, x)).ToArray() : rawTerms;
        }

        protected virtual string[] GetRawTerms(UIComponentMetadata metadata)
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
            TermCase termCase = GetTermCase(metadata);
            string name = GetPropertyName(metadata);
            return termCase.ApplyTo(name);
        }

        private TermCase GetTermCase(UIComponentMetadata metadata)
        {
            return this.GetCaseOrNull()
                ?? metadata.GetTerm().GetCaseOrNull()
                ?? GetSettingsAtribute(metadata).GetCaseOrNull()
                ?? DefaultCase;
        }

        private string GetTermFormat(UIComponentMetadata metadata)
        {
            return this.GetFormatOrNull()
                ?? metadata.GetTerm().GetFormatOrNull()
                ?? GetSettingsAtribute(metadata).GetFormatOrNull();
        }

        public TermMatch GetTermMatch(UIComponentMetadata metadata)
        {
            return this.GetMatchOrNull()
                ?? metadata.GetTerm().GetMatchOrNull()
                ?? GetSettingsAtribute(metadata).GetMatchOrNull()
                ?? DefaultMatch;
        }

        private string GetPropertyName(UIComponentMetadata metadata)
        {
            return CutEnding && (metadata.GetTerm()?.CutEnding ?? true)
                ? metadata.ComponentDefinitonAttribute.NormalizeNameIgnoringEnding(metadata.Name)
                : metadata.Name;
        }

        private TermFindSettingsAttribute GetSettingsAtribute(UIComponentMetadata metadata)
        {
            Type thisType = GetType();
            return metadata.GetFirstOrDefaultGlobalAttribute<TermFindSettingsAttribute>(x => x.FinderAttributeType == thisType);
        }
    }
}
