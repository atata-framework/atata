namespace Atata;

/// <summary>
/// Represents the behavior for control text typing by invoking <see cref="IWebElement.SendKeys(string)"/> method
/// for character by character with interval defined in <see cref="TypingIntervalInSeconds"/> property.
/// </summary>
public class TypesTextUsingSendKeysCharByCharAttribute : TextTypeBehaviorAttribute
{
    /// <summary>
    /// Gets or sets the typing interval in seconds.
    /// The default value is <c>0.2</c>.
    /// </summary>
    public double TypingIntervalInSeconds { get; set; } = 0.2;

    /// <inheritdoc/>
    public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
    {
        if (!string.IsNullOrEmpty(value))
        {
            var scopeElement = component.Scope;

            for (int i = 0; i < value.Length; i++)
            {
                if (i > 0 && TypingIntervalInSeconds > 0)
                    component.Owner.WaitSeconds(TypingIntervalInSeconds);

                scopeElement.SendKeysWithLogging(component.Session.Log, value[i].ToString());
            }
        }
    }
}
