namespace Atata
{
    /// <summary>
    /// Represents the hidden input control (&lt;input type="hidden"&gt;). Default search finds the first occurring &lt;input type="hidden"&gt; element.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='hidden']", Visibility = Visibility.Hidden)]
    public class HiddenInput<T, TOwner> : Input<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
