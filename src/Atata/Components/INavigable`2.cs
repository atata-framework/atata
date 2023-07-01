namespace Atata;

public interface INavigable<TNavigateTo, TOwner> : IControl<TOwner>
    where TNavigateTo : PageObject<TNavigateTo>
    where TOwner : PageObject<TOwner>
{
}
