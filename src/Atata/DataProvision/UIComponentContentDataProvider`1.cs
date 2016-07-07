namespace Atata
{
    public class UIComponentContentDataProvider<TOwner> : IUIComponentDataProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly UIComponent<TOwner> component;

        public UIComponentContentDataProvider(UIComponent<TOwner> component)
        {
            this.component = component;
        }

        string IUIComponentDataProvider<string, TOwner>.ComponentFullName
        {
            get { return component.ComponentFullName; }
        }

        TOwner IUIComponentDataProvider<string, TOwner>.Owner
        {
            get { return component.Owner; }
        }

        string IUIComponentDataProvider<string, TOwner>.ProviderName
        {
            get { return "content"; }
        }

        string IUIComponentDataProvider<string, TOwner>.ConvertValueToString(string value)
        {
            return value;
        }

        public string Get()
        {
            return component.Scope.Text;
        }
    }
}
