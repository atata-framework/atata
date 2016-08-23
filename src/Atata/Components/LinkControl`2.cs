namespace Atata
{
    public class LinkControl<TNavigateTo, TOwner> : LinkControl<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
