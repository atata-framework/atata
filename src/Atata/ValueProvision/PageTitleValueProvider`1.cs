namespace Atata
{
    public class PageTitleValueProvider<TOwner> : IUIComponentValueProvider<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public PageTitleValueProvider(TOwner owner)
        {
            Owner = owner;
        }

        public string ComponentFullName
        {
            get { return Owner.ComponentFullName; }
        }

        public TOwner Owner { get; private set; }

        public string ProviderName
        {
            get { return "title"; }
        }

        public string ConvertValueToString(string value)
        {
            return TermResolver.ToDisplayString(value);
        }

        public string Get()
        {
            return Owner.Driver.Title;
        }
    }
}
