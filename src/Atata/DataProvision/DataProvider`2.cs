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
            this.component = component.CheckNotNull("component");
            this.valueGetFunction = valueGetFunction.CheckNotNull("valueGetFunction");
            this.providerName = providerName.CheckNotNullOrWhitespace("providerName");
        }

        string IDataProvider<TData, TOwner>.ComponentFullName
        {
            get { return component.ComponentFullName; }
        }

        TOwner IDataProvider<TData, TOwner>.Owner
        {
            get { return component.Owner; }
        }

        string IDataProvider<TData, TOwner>.ProviderName
        {
            get { return providerName; }
        }

        public DataVerificationProvider<TData, TOwner> Should => new DataVerificationProvider<TData, TOwner>(this);

        string IDataProvider<TData, TOwner>.ConvertValueToString(TData value)
        {
            return TermResolver.ToString(value);
        }

        public TData Get()
        {
            return valueGetFunction();
        }
    }
}
