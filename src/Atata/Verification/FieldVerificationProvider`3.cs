namespace Atata
{
    public class FieldVerificationProvider<TData, TField, TOwner> :
        UIComponentVerificationProvider<TField, FieldVerificationProvider<TData, TField, TOwner>, TOwner>,
        IFieldVerificationProvider<TData, TField, TOwner>
        where TField : Field<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public FieldVerificationProvider(TField control)
            : base(control)
        {
        }

        public new NegationFieldVerificationProvider Not => new NegationFieldVerificationProvider(Component, this);

        IDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => Component;

        public class NegationFieldVerificationProvider :
            NegationControlVerificationProvider,
            IFieldVerificationProvider<TData, TField, TOwner>
        {
            internal NegationFieldVerificationProvider(TField control, IVerificationProvider<TOwner> verificationProvider)
                : base(control, verificationProvider)
            {
            }

            IDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => Component;
        }
    }
}
