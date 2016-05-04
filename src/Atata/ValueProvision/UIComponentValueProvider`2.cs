using System;

namespace Atata
{
    public class UIComponentValueProvider<TValue, TOwner> : IUIComponentValueProvider<TValue, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;
        private readonly Func<UIComponent<TOwner>, TValue> valueGetFunction;
        private readonly string providerName;

        public UIComponentValueProvider(UIComponent<TOwner> component, Func<UIComponent<TOwner>, TValue> valueGetFunction, string providerName)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (valueGetFunction == null)
                throw new ArgumentNullException("valueGetFunction");
            if (providerName == null)
                throw new ArgumentNullException("providerName");

            this.component = component;
            this.valueGetFunction = valueGetFunction;
            this.providerName = providerName;
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
            return valueGetFunction(component);
        }
    }
}
