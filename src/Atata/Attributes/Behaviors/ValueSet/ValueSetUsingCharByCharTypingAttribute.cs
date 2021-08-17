using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by clicking on the control element
    /// and then typing the text character by character with interval defined in <see cref="SetsValueUsingCharByCharTypingAttribute.TypingIntervalInSeconds"/> property.
    /// </summary>
    [Obsolete("Use " + nameof(SetsValueUsingCharByCharTypingAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueSetUsingCharByCharTypingAttribute : SetsValueUsingCharByCharTypingAttribute
    {
    }
}
