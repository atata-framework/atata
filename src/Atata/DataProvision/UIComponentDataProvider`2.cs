using System;

namespace Atata
{
    public class UIComponentDataProvider<TValue, TOwner> : IUIComponentDataProvider<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;
        private readonly Func<TValue> valueGetFunction;
        private readonly string providerName;

        public UIComponentDataProvider(UIComponent<TOwner> component, Func<TValue> valueGetFunction, string providerName)
        {
            this.component = component.CheckNotNull("component");
            this.valueGetFunction = valueGetFunction.CheckNotNull("valueGetFunction");
            this.providerName = providerName.CheckNotNullOrWhitespace("providerName");
        }

        string IUIComponentDataProvider<TValue, TOwner>.ComponentFullName
        {
            get { return component.ComponentFullName; }
        }

        TOwner IUIComponentDataProvider<TValue, TOwner>.Owner
        {
            get { return component.Owner; }
        }

        string IUIComponentDataProvider<TValue, TOwner>.ProviderName
        {
            get { return providerName; }
        }

        string IUIComponentDataProvider<TValue, TOwner>.ConvertValueToString(TValue value)
        {
            return TermResolver.ToString(value);
        }

        public TValue Get()
        {
            return valueGetFunction();
        }
    }
}
