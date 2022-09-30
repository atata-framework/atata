namespace Atata
{
    /// <summary>
    /// Represents the <see cref="Clickable{TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="Clickable{TOwner}" />
    public delegate TOwner ClickableDelegate<TOwner>()
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="Clickable{TNavigateTo, TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="Clickable{TNavigateTo, TOwner}" />
    public delegate TNavigateTo ClickableDelegate<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="Link{TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="Link{TOwner}" />
    public delegate TOwner LinkDelegate<TOwner>()
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="Link{TNavigateTo, TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="Link{TNavigateTo, TOwner}" />
    public delegate TNavigateTo LinkDelegate<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="Button{TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the owner page object.</returns>
    /// <seealso cref="Button{TOwner}" />
    public delegate TOwner ButtonDelegate<TOwner>()
        where TOwner : PageObject<TOwner>;

    /// <summary>
    /// Represents the <see cref="Button{TNavigateTo, TOwner}"/> delegate.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <returns>The instance of the <typeparamref name="TNavigateTo"/> page object.</returns>
    /// <seealso cref="Button{TNavigateTo, TOwner}" />
    public delegate TNavigateTo ButtonDelegate<TNavigateTo, TOwner>()
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>;
}
