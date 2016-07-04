namespace Atata
{
    public class DataVerificationProvider<TData, TOwner>
        : VerificationProvider<TOwner>, IDataVerificationProvider<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public DataVerificationProvider(IUIComponentValueProvider<TData, TOwner> dataProvider)
        {
            DataProvider = dataProvider;
        }

        protected IUIComponentValueProvider<TData, TOwner> DataProvider { get; private set; }

        IUIComponentValueProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => DataProvider;

        protected override TOwner Owner
        {
            get { return DataProvider.Owner; }
        }

        public NegationDataVerificationProvider Not => new NegationDataVerificationProvider(DataProvider);

        public class NegationDataVerificationProvider
            : NegationVerificationProvider<TOwner>, IDataVerificationProvider<TData, TOwner>
        {
            public NegationDataVerificationProvider(IUIComponentValueProvider<TData, TOwner> dataProvider)
            {
                DataProvider = dataProvider;
            }

            protected IUIComponentValueProvider<TData, TOwner> DataProvider { get; private set; }

            IUIComponentValueProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => DataProvider;

            protected override TOwner Owner
            {
                get { return DataProvider.Owner; }
            }
        }
    }
}
