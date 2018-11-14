namespace Atata
{
    /// <summary>
    /// Represents the password input control (<c>&lt;input type="password"&gt;</c>).
    /// Default search is performed by the label.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("input[@type='password']")]
    public class PasswordInput<TOwner> : Input<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
