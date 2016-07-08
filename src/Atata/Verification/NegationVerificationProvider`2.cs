namespace Atata
{
    public abstract class NegationVerificationProvider<TVerificationProvider, TOwner> : VerificationProvider<TVerificationProvider, TOwner>
        where TVerificationProvider : VerificationProvider<TVerificationProvider, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected NegationVerificationProvider()
            : base(isNegation: true)
        {
        }
    }
}
