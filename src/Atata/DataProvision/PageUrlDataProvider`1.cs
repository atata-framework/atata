namespace Atata
{
    public class PageUrlDataProvider<TOwner> : IUIComponentDataProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        private readonly TOwner pageObject;

        public PageUrlDataProvider(TOwner pageObject)
        {
            this.pageObject = pageObject;
        }

        string IUIComponentDataProvider<string, TOwner>.ComponentFullName
        {
            get { return pageObject.ComponentFullName; }
        }

        TOwner IUIComponentDataProvider<string, TOwner>.Owner
        {
            get { return pageObject; }
        }

        string IUIComponentDataProvider<string, TOwner>.ProviderName
        {
            get { return "URL"; }
        }

        string IUIComponentDataProvider<string, TOwner>.ConvertValueToString(string value)
        {
            return value;
        }

        public string Get()
        {
            return pageObject.Driver.Url;
        }
    }
}
