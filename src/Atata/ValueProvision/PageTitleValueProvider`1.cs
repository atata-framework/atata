namespace Atata
{
    public class PageTitleValueProvider<TOwner> : ValueProviderBase<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public PageTitleValueProvider(TOwner owner)
            : base(owner, owner.ComponentFullName)
        {
        }

        public override string ProviderName
        {
            get { return "page title"; }
        }

        public override string Get()
        {
            return Owner.Driver.Title;
        }
    }
}
