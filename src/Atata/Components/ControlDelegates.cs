namespace Atata
{
    public delegate TOwner Clickable<TOwner>()
        where TOwner : PageObject<TOwner>;

    public delegate TNavigateTo Clickable<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;

    public delegate TOwner Link<TOwner>()
        where TOwner : PageObject<TOwner>;

    public delegate TNavigateTo Link<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;

    public delegate TOwner Button<TOwner>()
        where TOwner : PageObject<TOwner>;

    public delegate TNavigateTo Button<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;
}
