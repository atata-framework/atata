namespace Atata
{
    public class FieldVerificationProvider<TValue, TField, TOwner> :
        UIComponentVerificationProvider<TField, FieldVerificationProvider<TValue, TField, TOwner>, TOwner>,
        IFieldVerificationProvider<TValue, TField, TOwner>
        where TField : Field<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public FieldVerificationProvider(TField control)
            : base(control)
        {
        }

        public NegationFieldVerificationProvider Not =>
            new NegationFieldVerificationProvider(Component, this);

        IObjectProvider<TValue, TOwner> IObjectVerificationProvider<TValue, TOwner>.ObjectProvider =>
            Component;

        public class NegationFieldVerificationProvider :
            NegationUIComponentVerificationProvider<NegationFieldVerificationProvider>,
            IFieldVerificationProvider<TValue, TField, TOwner>
        {
            internal NegationFieldVerificationProvider(TField control, IVerificationProvider<TOwner> verificationProvider)
                : base(control, verificationProvider)
            {
            }

            IObjectProvider<TValue, TOwner> IObjectVerificationProvider<TValue, TOwner>.ObjectProvider =>
                Component;
        }
    }
}
