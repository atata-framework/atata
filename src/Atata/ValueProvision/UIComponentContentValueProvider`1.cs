namespace Atata
{
    public class UIComponentContentValueProvider<TOwner> : IUIComponentValueProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;

        public UIComponentContentValueProvider(UIComponent<TOwner> component)
        {
            this.component = component;
        }

        string IUIComponentValueProvider<string, TOwner>.ComponentFullName
        {
            get { return component.ComponentFullName; }
        }

        TOwner IUIComponentValueProvider<string, TOwner>.Owner
        {
            get { return component.Owner; }
        }

        string IUIComponentValueProvider<string, TOwner>.ProviderName
        {
            get { return "content"; }
        }

        string IUIComponentValueProvider<string, TOwner>.ConvertValueToString(string value)
        {
            return value;
        }

        public string Get()
        {
            return component.Scope.Text;
        }
    }
}
