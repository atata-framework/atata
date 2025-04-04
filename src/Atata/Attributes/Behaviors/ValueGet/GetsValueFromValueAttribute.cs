#nullable enable

namespace Atata;

/// <summary>
/// Represents the behavior for control value getting from <c>value</c> DOM property.
/// </summary>
public class GetsValueFromValueAttribute : ValueGetBehaviorAttribute
{
    /// <inheritdoc/>
    public override string Execute<TOwner>(IUIComponent<TOwner> component) =>
        component.DomProperties.Value.Value ?? string.Empty;
}
