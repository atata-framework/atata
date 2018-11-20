namespace Atata
{
    /// <summary>
    /// Represents the button control.
    /// Default search is performed by the content and value (button by content text and input by value attribute).
    /// Handles any input element with <c>type="button"</c>, <c>type="submit"</c>, <c>type="reset"</c> or <c>button</c> element.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="Button{TOwner}" />
    /// <seealso cref="INavigable{TNavigateTo, TOwner}" />
    public class Button<TNavigateTo, TOwner> : Button<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
