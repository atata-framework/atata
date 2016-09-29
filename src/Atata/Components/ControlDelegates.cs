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

    /// <summary>
    /// Represents the <see cref="ButtonControl{TOwner}"/> delegate. By default is being searched by the content and value (button by content text and input by value attribute). Handles any input element with type="button", type="submit", type="reset" or button element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    public delegate TOwner Button<TOwner>()
        where TOwner : PageObject<TOwner>;

    public delegate TNavigateTo Button<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;
}
