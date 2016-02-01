namespace Atata
{
    public class Link<TNavigateTo, TOwner> : Link<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            return ClickAndGoTo<TNavigateTo>();
        }
    }
}
