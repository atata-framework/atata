using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by executing
    /// <c>HTMLElement.value = '{value}'; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
    /// </summary>
    [Obsolete("Use " + nameof(SetsValueUsingScriptAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueSetUsingScriptAttribute : SetsValueUsingScriptAttribute
    {
    }
}
