namespace Atata
{
    public abstract class TermFindItemAttribute : FindItemAttribute
    {
        protected TermFindItemAttribute()
        {
        }

        protected TermFindItemAttribute(TermCase termCase)
        {
            Case = termCase;
        }

        protected TermFindItemAttribute(TermMatch match)
        {
            Match = match;
        }

        protected TermFindItemAttribute(TermMatch match, TermCase termCase)
        {
            Match = match;
            Case = termCase;
        }

        /// <summary>
        /// Gets the match.
        /// </summary>
        public new TermMatch Match
        {
            get => Properties.Get(nameof(Match), TermMatch.Equals);
            private set => Properties[nameof(Match)] = value;
        }

        /// <summary>
        /// Gets the term case.
        /// </summary>
        public TermCase Case
        {
            get => Properties.Get(nameof(Case), TermCase.None);
            private set => Properties[nameof(Case)] = value;
        }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format
        {
            get => Properties.Get<string>(nameof(Format));
            set => Properties[nameof(Format)] = value;
        }
    }
}
