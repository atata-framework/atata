namespace Atata
{
    public class FieldVerificationProvider<TData, TField, TOwner> :
        ControlVerificationProvider<TField, FieldVerificationProvider<TData, TField, TOwner>, TOwner>,
        IDataVerificationProvider<TData, TOwner>
        where TField : Field<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public FieldVerificationProvider(TField control)
            : base(control)
        {
        }

        public new NegationFieldVerificationProvider Not => new NegationFieldVerificationProvider(Control);

        IDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => Control;

        public class NegationFieldVerificationProvider
            : NegationControlVerificationProvider, IDataVerificationProvider<TData, TOwner>
        {
            public NegationFieldVerificationProvider(TField control)
                : base(control)
            {
            }

            IDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => Control;
        }
    }
}
