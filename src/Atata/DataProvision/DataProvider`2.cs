using System;

namespace Atata
{
    /// <summary>
    /// Represents the provider of UI component data of <typeparamref name="TData"/> type
    /// owned by <typeparamref name="TOwner"/> object.
    /// </summary>
    /// <typeparam name="TData">The type of the data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner.</typeparam>
    public class DataProvider<TData, TOwner> : IDataProvider<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly Func<TData> _valueGetFunction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataProvider{TData, TOwner}"/> class.
        /// </summary>
        /// <param name="component">The associated component.</param>
        /// <param name="valueGetFunction">The function that gets the value.</param>
        /// <param name="providerName">Name of the provider.</param>
        public DataProvider(UIComponent<TOwner> component, Func<TData> valueGetFunction, string providerName)
        {
            Component = component.CheckNotNull(nameof(component));
            _valueGetFunction = valueGetFunction.CheckNotNull(nameof(valueGetFunction));
            ProviderName = providerName.CheckNotNullOrWhitespace(nameof(providerName));
        }

        /// <summary>
        /// Gets the associated component.
        /// </summary>
        protected UIComponent<TOwner> Component { get; }

        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        protected string ProviderName { get; }

        UIComponent IDataProvider<TData, TOwner>.Component => Component;

        TOwner IDataProvider<TData, TOwner>.Owner => Component.Owner;

        string IObjectProvider<TData>.ProviderName => ProviderName;

        TermOptions IDataProvider<TData, TOwner>.ValueTermOptions { get; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public TData Value
        {
            get { return _valueGetFunction(); }
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
    }
}
