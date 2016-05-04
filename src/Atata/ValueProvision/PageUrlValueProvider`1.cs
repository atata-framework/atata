namespace Atata
{
    public class PageUrlValueProvider<TOwner> : IUIComponentValueProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly TOwner pageObject;

        public PageUrlValueProvider(TOwner pageObject)
        {
            this.pageObject = pageObject;
        }

        string IUIComponentValueProvider<string, TOwner>.ComponentFullName
        {
            get { return pageObject.ComponentFullName; }
        }

        TOwner IUIComponentValueProvider<string, TOwner>.Owner
        {
            get { return pageObject; }
        }

        string IUIComponentValueProvider<string, TOwner>.ProviderName
        {
            get { return "URL"; }
        }

        string IUIComponentValueProvider<string, TOwner>.ConvertValueToString(string value)
        {
            return value;
        }

        public string Get()
        {
            return pageObject.Driver.Url;
        }
    }
}
