namespace Atata
{
    public delegate TOwner _Clickable<TOwner>()
        where TOwner : PageObject<TOwner>;

    public delegate TNavigateTo _Clickable<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;

    public delegate TOwner _Link<TOwner>()
        where TOwner : PageObject<TOwner>;

    public delegate TNavigateTo _Link<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;
}
