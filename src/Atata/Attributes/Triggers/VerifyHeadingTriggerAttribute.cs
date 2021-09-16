namespace Atata
{
    public abstract class VerifyHeadingTriggerAttribute : TermVerificationTriggerAttribute
    {
        protected VerifyHeadingTriggerAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        protected VerifyHeadingTriggerAttribute(TermMatch match, TermCase termCase)
            : base(match, termCase)
        {
        }

        protected VerifyHeadingTriggerAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        protected VerifyHeadingTriggerAttribute(params string[] values)
            : base(values)
        {
        }

        /// <summary>
        /// Gets or sets the index of header.
        /// The default value is <c>-1</c>, meaning that the index is not used.
        /// </summary>
        public int Index
        {
            get => Properties.Get(nameof(Index), -1, GetPropertySettings);
            set => Properties[nameof(Index)] = value;
        }

        protected void Verify<TH, TOwner>(TriggerContext<TOwner> context, string[] values)
            where TH : Content<string, TOwner>
            where TOwner : PageObject<TOwner>
        {
            if (Index >= 0)
            {
                var headingControl = context.Component.Owner.Find<TH>(
                    new FindByIndexAttribute(Index) { Visibility = Visibility.Visible });

                headingControl.Should.MatchAny(Match, values);
            }
            else
            {
                var headingControl = context.Component.Owner.Find<TH>(
                    Match.FormatComponentName(values),
                    new FindByContentAttribute(Match, values) { Visibility = Visibility.Visible });

                headingControl.Should.BePresent();
            }
        }
    }
}
