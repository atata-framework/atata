namespace Atata
{
    public class ButtonControl<TNavigateTo, TOwner> : ButtonControl<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
