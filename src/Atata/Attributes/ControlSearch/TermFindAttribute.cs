using System;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes that use terms.
    /// </summary>
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
        }

        /// <summary>
        /// Gets the term values.
        /// </summary>
        public string[] Values { get; private set; }

        /// <summary>
        /// Gets the term case.
        /// </summary>
        public TermCase Case { get; private set; }

        /// <summary>
        /// Gets the match.
        /// </summary>
        public new TermMatch Match { get; private set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the name should be cut considering the IgnoreNameEndings property value of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>. The default value is true.
        /// </summary>
        public bool CutEnding { get; set; } = true;

        /// <summary>
        /// Gets the default term case.
        /// </summary>
        protected abstract TermCase DefaultCase { get; }

        /// <summary>
        /// Gets the default match. The default value is Equals.
        /// </summary>
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
                ?? GetTermFindSettings(metadata, x => x.Case != TermCase.Inherit).GetCaseOrNull()
                ?? DefaultCase;
        }

        private string GetTermFormat(UIComponentMetadata metadata)
        {
            return this.GetFormatOrNull()
                ?? metadata.GetTerm().GetFormatOrNull()
                ?? GetTermFindSettings(metadata, x => x.Format != null).GetFormatOrNull();
        }

        public TermMatch GetTermMatch(UIComponentMetadata metadata)
        {
            return this.GetMatchOrNull()
                ?? metadata.GetTerm().GetMatchOrNull()
                ?? GetTermFindSettings(metadata, x => x.Match != TermMatch.Inherit).GetMatchOrNull()
                ?? DefaultMatch;
        }

        private string GetPropertyName(UIComponentMetadata metadata)
        {
            return CutEnding && (metadata.GetTerm()?.CutEnding ?? true)
                ? metadata.ComponentDefinitonAttribute.NormalizeNameIgnoringEnding(metadata.Name)
                : metadata.Name;
        }

        private TermFindSettingsAttribute GetTermFindSettings(UIComponentMetadata metadata, Func<TermFindSettingsAttribute, bool> predicate)
        {
            Type thisType = GetType();
            return metadata.GetFirstOrDefaultGlobalAttribute<TermFindSettingsAttribute>(x => x.FindAttributeType == thisType && predicate(x));
        }
    }
}
