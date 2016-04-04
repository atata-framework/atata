namespace Atata
{
    public class LinkControl<TNavigateTo, TOwner> : LinkControl<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            return ClickAndGoTo<TNavigateTo>();
        }
    }
}
