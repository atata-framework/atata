namespace Atata;

/// <summary>
/// Represents the behavior for control text typing by <see cref="IWebElement.SendKeys(string)"/> method.
/// <see cref="IWebElement.SendKeys(string)"/> method is invoked only when the value is not null or empty.
/// </summary>
public class TypesTextUsingSendKeysAttribute : TextTypeBehaviorAttribute
{
    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
    {
        if (value?.Length > 0)
            component.Scope.SendKeysWithLogging(component.Session.Log, value);
    }
}
