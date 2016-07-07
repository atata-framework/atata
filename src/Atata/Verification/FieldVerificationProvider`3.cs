namespace Atata
{
    public class FieldVerificationProvider<TData, TField, TOwner>
        : ControlVerificationProvider<TField, TOwner>, IDataVerificationProvider<TData, TOwner>
        where TField : Field<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public FieldVerificationProvider(TField control)
            : base(control)
        {
        }

        public new NegationFieldVerificationProvider Not => new NegationFieldVerificationProvider(Control);

        IUIComponentDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => Control;

        public class NegationFieldVerificationProvider
            : NegationControlVerificationProvider, IDataVerificationProvider<TData, TOwner>
        {
            public NegationFieldVerificationProvider(TField control)
                : base(control)
            {
            }

            IUIComponentDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => Control;
        }
    }
}
