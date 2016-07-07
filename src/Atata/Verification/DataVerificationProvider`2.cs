namespace Atata
{
    public class DataVerificationProvider<TData, TOwner>
        : VerificationProvider<TOwner>, IDataVerificationProvider<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public DataVerificationProvider(IUIComponentDataProvider<TData, TOwner> dataProvider)
        {
            DataProvider = dataProvider;
        }

        protected IUIComponentDataProvider<TData, TOwner> DataProvider { get; private set; }

        IUIComponentDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => DataProvider;

        protected override TOwner Owner
        {
            get { return DataProvider.Owner; }
        }

        public NegationDataVerificationProvider Not => new NegationDataVerificationProvider(DataProvider);

        public class NegationDataVerificationProvider
            : NegationVerificationProvider<TOwner>, IDataVerificationProvider<TData, TOwner>
        {
            public NegationDataVerificationProvider(IUIComponentDataProvider<TData, TOwner> dataProvider)
            {
                DataProvider = dataProvider;
            }

            protected IUIComponentDataProvider<TData, TOwner> DataProvider { get; private set; }

            IUIComponentDataProvider<TData, TOwner> IDataVerificationProvider<TData, TOwner>.DataProvider => DataProvider;

            protected override TOwner Owner
            {
                get { return DataProvider.Owner; }
            }
        }
    }
}
