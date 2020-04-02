namespace Atata
{
    /// <summary>
    /// Represents the hidden input control (<c>&lt;input type="hidden"&gt;</c>).
    /// Default search finds the first occurring <c>&lt;input type="hidden"&gt;</c> element.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='hidden']", Visibility = Visibility.Hidden)]
    [FindFirst]
    public class HiddenInput<T, TOwner> : Input<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
