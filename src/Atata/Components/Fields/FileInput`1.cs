namespace Atata;

/// <summary>
/// Represents the file input control (<c>&lt;input type="file"&gt;</c>).
/// Default search is performed by the label.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition("input[@type='file']", Visibility = Visibility.Any, ComponentTypeName = "file input")]
[SetsValueUsingSendKeys]
[ClearsValueUsingClearMethodOrScript]
public class FileInput<TOwner> : Input<string, TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the <c>accept</c> DOM property.
    /// </summary>
    public ValueProvider<string?, TOwner> Accept =>
        DomProperties["accept"];
}
