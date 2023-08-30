namespace Atata;

/// <summary>
/// Represents the JavaScript alert box.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public sealed class AlertBox<TOwner> : PopupBox<AlertBox<TOwner>, TOwner>
    where TOwner : PageObject<TOwner>
{
    internal AlertBox(TOwner owner)
        : base(owner)
    {
    }

    private protected override string KindName => "alert box";
}
