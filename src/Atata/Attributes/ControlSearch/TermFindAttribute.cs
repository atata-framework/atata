using System;
using System.Collections.Generic;
using System.Linq;

namespace Atata
{
    /// <summary>
    /// Represents the base attribute class for the finding attributes that use terms.
    /// </summary>
    public abstract class TermFindAttribute : FindAttribute, ITermFindAttribute, ITermMatchFindAttribute, ITermSettings
    {
        private readonly Func<UIComponentMetadata, IEnumerable<IPropertySettings>> termGetter;

        private readonly Func<UIComponentMetadata, IEnumerable<IPropertySettings>> termFindSettingsGetter;

        protected TermFindAttribute(TermCase termCase)
            : this()
        {
            Case = termCase;
        }

        protected TermFindAttribute(TermMatch match, TermCase termCase)
            : this()
        {
            Match = match;
            Case = termCase;
        }

        protected TermFindAttribute(TermMatch match, params string[] values)
            : this(values)
        {
            Match = match;
        }

        protected TermFindAttribute(params string[] values)
        {
            if (values != null && values.Any())
                Values = values;

            termGetter = md => md.GetAll<TermAttribute>();

            termFindSettingsGetter = md => md.GetAll<TermFindSettingsAttribute>(x => x.ForAttribute(GetType()));
        }

        /// <summary>
        /// Gets the term values.
        /// </summary>
        public string[] Values
        {
            get
            {
                return Properties.Get<string[]>(
                    nameof(Values),
                    termGetter);
            }

            private set
            {
                Properties[nameof(Values)] = value;
            }
        }

        /// <summary>
        /// Gets the term case.
        /// </summary>
        public TermCase Case
        {
            get
            {
                return Properties.Get(
                    nameof(Case),
                    DefaultCase,
                    termGetter,
                    termFindSettingsGetter);
            }

            private set
            {
                Properties[nameof(Case)] = value;
            }
        }

        /// <summary>
        /// Gets the match.
        /// </summary>
        public new TermMatch Match
        {
            get
            {
                return Properties.Get(
                    nameof(Match),
                    DefaultMatch,
                    termGetter,
                    termFindSettingsGetter);
            }

            private set
            {
                Properties[nameof(Match)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format
        {
            get
            {
                return Properties.Get<string>(
                    nameof(Format),
                    termGetter,
                    termFindSettingsGetter);
            }

            set
            {
                Properties[nameof(Format)] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the name should be cut considering the <see cref="UIComponentDefinitionAttribute.IgnoreNameEndings"/> property value of <see cref="ControlDefinitionAttribute"/> and <see cref="PageObjectDefinitionAttribute"/>.
        /// The default value is <c>true</c>.
        /// </summary>
        public bool CutEnding
        {
            get
            {
                return Properties.Get(
                    nameof(CutEnding),
                    true,
                    termGetter,
                    termFindSettingsGetter);
            }

            set
            {
                Properties[nameof(CutEnding)] = value;
            }
        }

        /// <summary>
        /// Gets the default term case.
        /// </summary>
        protected abstract TermCase DefaultCase { get; }

        /// <summary>
        /// Gets the default match.
        /// The default value is <see cref="TermMatch.Equals"/>.
        /// </summary>
        protected virtual TermMatch DefaultMatch
        {
            get { return TermMatch.Equals; }
        }

        public string[] GetTerms(UIComponentMetadata metadata)
        {
            string[] rawTerms = GetRawTerms(metadata);
            string format = Format;

            return !string.IsNullOrEmpty(format) ? rawTerms.Select(x => string.Format(format, x)).ToArray() : rawTerms;
        }

        protected virtual string[] GetRawTerms(UIComponentMetadata metadata)
        {
            return Values ?? new[] { GetTermFromProperty(metadata) };
        }

        private string GetTermFromProperty(UIComponentMetadata metadata)
        {
            string name = GetPropertyName(metadata);
            return Case.ApplyTo(name);
        }

        public TermMatch GetTermMatch(UIComponentMetadata metadata)
        {
            return Match;
        }

        private string GetPropertyName(UIComponentMetadata metadata)
        {
            return CutEnding
                ? metadata.ComponentDefinitionAttribute.NormalizeNameIgnoringEnding(metadata.Name)
                : metadata.Name;
        }
    }
}
