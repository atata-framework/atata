namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by clicking on the control element
    /// and then typing the text character by character with interval defined in <see cref="TypingIntervalInSeconds"/> property.
    /// </summary>
    public class ValueSetUsingCharByCharTypingAttribute : ValueSetBehaviorAttribute
    {
        /// <summary>
        /// Gets the typing interval in seconds.
        /// The default value is <c>0.2</c>.
        /// </summary>
        public double TypingIntervalInSeconds { get; } = 0.2;

        public override void Execute<TOwner>(IUIComponent<TOwner> component, string value)
        {
            var scopeElement = component.Scope;

            scopeElement.ClickWithLogging();

            if (!string.IsNullOrEmpty(value))
            {
                foreach (char character in value)
                {
                    component.Owner.Wait(TypingIntervalInSeconds);
                    scopeElement.SendKeysWithLogging(character.ToString());
                }
            }
        }
    }
}
