using System;

namespace Atata
{
    public class UIComponentValueProvider<TValue, TOwner> : IUIComponentValueProvider<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;
        private readonly Func<TValue> valueGetFunction;
        private readonly string providerName;

        public UIComponentValueProvider(UIComponent<TOwner> component, Func<TValue> valueGetFunction, string providerName)
        {
            this.component = component.CheckNotNull("component");
            this.valueGetFunction = valueGetFunction.CheckNotNull("valueGetFunction");
            this.providerName = providerName.CheckNotNullOrWhitespace("providerName");
        }

        string IUIComponentValueProvider<TValue, TOwner>.ComponentFullName
        {
            get { return component.ComponentFullName; }
        }

        TOwner IUIComponentValueProvider<TValue, TOwner>.Owner
        {
            get { return component.Owner; }
        }

        string IUIComponentValueProvider<TValue, TOwner>.ProviderName
        {
            get { return providerName; }
        }

        string IUIComponentValueProvider<TValue, TOwner>.ConvertValueToString(TValue value)
        {
            return TermResolver.ToString(value);
        }

        public TValue Get()
        {
            return valueGetFunction();
        }
    }
}
