namespace Atata
{
    public abstract class VerifyHeadingTriggerAttribute : TermVerificationTriggerAttribute
    {
        protected VerifyHeadingTriggerAttribute(TermCase termCase)
            : base(termCase)
        {
        }

        protected VerifyHeadingTriggerAttribute(TermMatch match, TermCase termCase = TermCase.Inherit)
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
        /// Gets or sets the index of header. The default value is -1, meaning that the index is not used.
        /// </summary>
        public int Index { get; set; } = -1;
    }
}
