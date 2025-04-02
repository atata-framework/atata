#nullable enable

namespace Atata;

/// <summary>
/// Represents the email input control (<c>&lt;input type="email"&gt;</c>).
/// Default search is performed by the label.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='email']", ComponentTypeName = "email input")]
public class EmailInput<TOwner> : Input<string, TOwner>
    where TOwner : PageObject<TOwner>
{
}
