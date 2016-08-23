namespace Atata
{
    public class ClickableControl<TNavigateTo, TOwner> : ClickableControl<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
