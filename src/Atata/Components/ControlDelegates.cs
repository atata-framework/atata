namespace Atata
{
    /// <summary>
    /// Represents the <see cref="ClickableControl{TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="ClickableControl{TOwner}" />
    public delegate TOwner Clickable<TOwner>()
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="ClickableControl{TNavigateTo, TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="ClickableControl{TNavigateTo, TOwner}" />
    public delegate TNavigateTo Clickable<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="LinkControl{TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="LinkControl{TOwner}" />
    public delegate TOwner Link<TOwner>()
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="LinkControl{TNavigateTo, TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="LinkControl{TNavigateTo, TOwner}" />
    public delegate TNavigateTo Link<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="ButtonControl{TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="ButtonControl{TOwner}" />
    public delegate TOwner Button<TOwner>()
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="ButtonControl{TNavigateTo, TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the <typeparamref name="TNavigateTo"/> page object.</returns>
    /// <seealso cref="ButtonControl{TNavigateTo, TOwner}" />
    public delegate TNavigateTo Button<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;
}
