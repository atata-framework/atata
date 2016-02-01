namespace Atata
{
    public class Button<TNavigateTo, TOwner> : Button<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            return ClickAndGoTo<TNavigateTo>();
        }
    }
}
