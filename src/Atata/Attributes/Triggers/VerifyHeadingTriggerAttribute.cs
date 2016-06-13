namespace Atata
{
    public abstract class VerifyHeadingTriggerAttribute : TermVerificationTriggerAttribute
    {
        protected VerifyHeadingTriggerAttribute(params string[] values)
            : base(values)
        {
        }

        protected VerifyHeadingTriggerAttribute(TermFormat format = TermFormat.Inherit)
            : base(format)
        {
        }

        protected VerifyHeadingTriggerAttribute(TermMatch match, params string[] values)
            : base(match, values)
        {
        }

        protected VerifyHeadingTriggerAttribute(TermMatch match, TermFormat format = TermFormat.Inherit)
            : base(match, format)
        {
        }

        public int Index { get; set; }
    }
}
