namespace Atata
{
    public class ClickableControl<TNavigateTo, TOwner> : ClickableControl<TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
        public new TNavigateTo Click()
        {
            return ClickAndGoTo<TNavigateTo>();
        }
    }
}
