using System;

namespace Atata
{
    public class DataProvider<TData, TOwner> : IDataProvider<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;
        private readonly Func<TData> valueGetFunction;
        private readonly string providerName;

        public DataProvider(UIComponent<TOwner> component, Func<TData> valueGetFunction, string providerName)
        {
            this.component = component.CheckNotNull(nameof(component));
            this.valueGetFunction = valueGetFunction.CheckNotNull(nameof(valueGetFunction));
            this.providerName = providerName.CheckNotNullOrWhitespace(nameof(providerName));
        }

        UIComponent IDataProvider<TData, TOwner>.Component
        {
            get { return component; }
        }

        TOwner IDataProvider<TData, TOwner>.Owner
        {
            get { return component.Owner; }
        }

        string IDataProvider<TData, TOwner>.ProviderName
        {
            get { return providerName; }
        }

        TermOptions IDataProvider<TData, TOwner>.ValueTermOptions { get; }

        public TData Value
        {
            get { return valueGetFunction(); }
        }

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public DataVerificationProvider<TData, TOwner> Should => new DataVerificationProvider<TData, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public DataVerificationProvider<TData, TOwner> ExpectTo => Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public DataVerificationProvider<TData, TOwner> WaitTo => Should.Using<WaitingVerificationStrategy>();

        public static implicit operator TData(DataProvider<TData, TOwner> dataProvider)
        {
            return dataProvider.Value;
        }

        [Obsolete("Use Value instead.")] // Obsolete since v1.0.0.
        public TData Get()
        {
            return Value;
        }
    }
}
