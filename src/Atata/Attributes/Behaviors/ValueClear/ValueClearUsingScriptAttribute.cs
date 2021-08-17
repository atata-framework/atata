using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by executing
    /// <c>HTMLElement.value = ''; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
    /// </summary>
    [Obsolete("Use " + nameof(ClearsValueUsingScriptAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueClearUsingScriptAttribute : ClearsValueUsingScriptAttribute
    {
    }
}
