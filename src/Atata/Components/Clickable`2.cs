namespace Atata
{
    public class Clickable<TNavigateTo, TOwner> : Clickable<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            return ClickAndGoTo<TNavigateTo>();
        }
    }
}
