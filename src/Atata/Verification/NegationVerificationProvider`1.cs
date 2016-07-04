namespace Atata
{
    public abstract class NegationVerificationProvider<TOwner> : VerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected NegationVerificationProvider()
            : base(isNegation: true)
        {
        }
    }
}
