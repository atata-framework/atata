using System;

namespace Atata
{
    public class UIComponentDataProvider<TData, TOwner> : IUIComponentDataProvider<TData, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;
        private readonly Func<TData> valueGetFunction;
        private readonly string providerName;

        public UIComponentDataProvider(UIComponent<TOwner> component, Func<TData> valueGetFunction, string providerName)
        {
            this.component = component.CheckNotNull("component");
            this.valueGetFunction = valueGetFunction.CheckNotNull("valueGetFunction");
            this.providerName = providerName.CheckNotNullOrWhitespace("providerName");
        }

        string IUIComponentDataProvider<TData, TOwner>.ComponentFullName
        {
            get { return component.ComponentFullName; }
        }

        TOwner IUIComponentDataProvider<TData, TOwner>.Owner
        {
            get { return component.Owner; }
        }

        string IUIComponentDataProvider<TData, TOwner>.ProviderName
        {
            get { return providerName; }
        }

        public DataVerificationProvider<TData, TOwner> Should => new DataVerificationProvider<TData, TOwner>(this);

        string IUIComponentDataProvider<TData, TOwner>.ConvertValueToString(TData value)
        {
            return TermResolver.ToString(value);
        }

        public TData Get()
        {
            return valueGetFunction();
        }
    }
}
