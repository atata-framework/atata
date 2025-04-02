#nullable enable

namespace Atata;

/// <summary>
/// Represents the hidden input control (<c>&lt;input type="hidden"&gt;</c>).
/// Default search finds the first occurring <c>&lt;input type="hidden"&gt;</c> element.
/// </summary>
/// <typeparam name="TValue">The type of the control's value.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='hidden']", Visibility = Visibility.Hidden, ComponentTypeName = "hidden input")]
[FindFirst]
public class HiddenInput<TValue, TOwner> : Input<TValue, TOwner>
    where TOwner : PageObject<TOwner>
{
}
