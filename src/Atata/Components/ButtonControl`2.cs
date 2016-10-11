namespace Atata
{
    /// <summary>
    /// Represents the button control. Default search is performed by the content and value (button by content text and input by value attribute). Handles any input element with type="button", type="submit", type="reset" or button element.
    /// </summary>
    /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <seealso cref="ButtonControl{TOwner}" />
    /// <seealso cref="INavigable{TNavigateTo, TOwner}" />
    public class ButtonControl<TNavigateTo, TOwner> : ButtonControl<TOwner>, INavigable<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>
        where TOwner : PageObject<TOwner>
    {
    }
}
