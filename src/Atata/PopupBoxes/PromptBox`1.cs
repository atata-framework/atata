namespace Atata;

/// <summary>
/// Represents the JavaScript prompt box.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public sealed class PromptBox<TOwner> : PopupBox<PromptBox<TOwner>, TOwner>
    where TOwner : PageObject<TOwner>
{
    internal PromptBox(TOwner owner)
        : base(owner)
    {
    }

    private protected override string KindName => "prompt box";

    /// <summary>
    /// Types the specified text value into the prompt box.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The same <see cref="PromptBox{TOwner}"/> instance.</returns>
    public PromptBox<TOwner> Type(string text)
    {
        string textForLog = SpecialKeys.Replace(text);

        Owner.Log.ExecuteSection(
            new LogSection($"Type \"{textForLog}\" in {KindName}"),
            () => Alert.SendKeys(text));

        return this;
    }

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
