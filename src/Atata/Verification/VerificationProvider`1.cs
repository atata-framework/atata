namespace Atata
{
    public abstract class VerificationProvider<TOwner> : IVerificationProvider<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly bool isNegation;

        protected VerificationProvider(bool isNegation = false)
        {
            this.isNegation = isNegation;
        }

        bool IVerificationProvider<TOwner>.IsNegation => isNegation;

        TOwner IVerificationProvider<TOwner>.Owner
        {
            get { return Owner; }
        }

        protected abstract TOwner Owner { get; }
    }
}
