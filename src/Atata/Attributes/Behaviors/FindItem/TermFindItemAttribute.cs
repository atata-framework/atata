namespace Atata
{
    public abstract class TermFindItemAttribute : FindItemAttribute, IHasOptionalProperties
    {
        protected TermFindItemAttribute()
        {
        }

        protected TermFindItemAttribute(TermCase termCase) =>
            Case = termCase;

        protected TermFindItemAttribute(TermMatch match) =>
            Match = match;

        protected TermFindItemAttribute(TermMatch match, TermCase termCase)
        {
            Match = match;
            Case = termCase;
        }

        PropertyBag IHasOptionalProperties.OptionalProperties => OptionalProperties;

        protected PropertyBag OptionalProperties { get; } = new PropertyBag();

        /// <summary>
        /// Gets the match.
        /// </summary>
        public new TermMatch Match
        {
            get => OptionalProperties.GetOrDefault(nameof(Match), TermMatch.Equals);
            private set => OptionalProperties[nameof(Match)] = value;
        }

        /// <summary>
        /// Gets the term case.
        /// </summary>
        public TermCase Case
        {
            get => OptionalProperties.GetOrDefault(nameof(Case), TermCase.None);
            private set => OptionalProperties[nameof(Case)] = value;
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format
        {
            get => OptionalProperties.GetOrDefault<string>(nameof(Format));
            set => OptionalProperties[nameof(Format)] = value;
        }
    }
}
