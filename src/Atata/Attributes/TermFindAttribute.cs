using System;
using System.Linq;

namespace Atata
{
    public abstract class TermFindAttribute : FindAttribute, ITermFindAttribute, ITermMatchFindAttribute
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
        public bool CutEnding { get; set; }

        protected abstract TermCase DefaultCase { get; }

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
            TermCase termCase = GetTermCase(metadata);
            string name = GetPropertyName(metadata);
            return termCase.ApplyTo(name);
        }

        private TermCase GetTermCase(UIComponentMetadata metadata)
        {
            TermAttribute termAttribute;
            if (Case != TermCase.Inherit)
                return Case;
            else if ((termAttribute = metadata.GetTerm(x => x.Case != TermCase.Inherit)) != null)
                return termAttribute.Case;
            else
                return GetTermCaseFromMetadata(metadata);
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

        private TermCase GetTermCaseFromMetadata(UIComponentMetadata metadata)
        {
            Type thisType = GetType();
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<TermFindSettingsAttribute>(x => x.FinderAttributeType == thisType && x.Case != TermCase.Inherit);
            return settingsAttribute != null ? settingsAttribute.Case : DefaultCase;
        }

        private TermMatch GetTermMatchFromMetadata(UIComponentMetadata metadata)
        {
            Type thisType = GetType();
            var settingsAttribute = metadata.GetFirstOrDefaultGlobalAttribute<TermFindSettingsAttribute>(x => x.FinderAttributeType == thisType && x.Match != TermMatch.Inherit);
            return settingsAttribute != null ? settingsAttribute.Match : DefaultMatch;
        }
    }
}
