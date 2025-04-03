#nullable enable

namespace Atata;

/// <summary>
/// Represents the JavaScript confirm box.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public sealed class ConfirmBox<TOwner> : PopupBox<ConfirmBox<TOwner>, TOwner>
    where TOwner : PageObject<TOwner>
{
    internal ConfirmBox(TOwner owner)
        : base(owner)
    {
    }

    private protected override string KindName => "confirm box";

    /// <summary>
    /// Cancels the popup box.
    /// </summary>
    /// <returns>The owner page object.</returns>
    public TOwner Cancel()
    {
        Owner.Log.ExecuteSection(
            new LogSection($"Cancel {KindName}"),
            Alert.Dismiss);

        return Owner;
    }
}
