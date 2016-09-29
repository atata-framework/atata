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

        public DataVerificationProvider<TData, TOwner> Should => new DataVerificationProvider<TData, TOwner>(this);

        public static implicit operator TData(DataProvider<TData, TOwner> field)
        {
            return field.Get();
        }

        public TData Get()
        {
            return valueGetFunction();
        }
    }
}
