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

        public string ComponentFullName
        {
            get { return component.ComponentFullName; }
        }

        public TOwner Owner
        {
            get { return component.Owner; }
        }

        public string ProviderName
        {
            get { return "content"; }
        }

        public string ConvertValueToString(string value)
        {
            return TermResolver.ToDisplayString(value);
        }

        public string Get()
        {
            return component.Scope.Text;
        }
    }
}
