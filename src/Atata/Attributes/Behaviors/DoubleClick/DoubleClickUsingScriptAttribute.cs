using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control double-clicking by executing <c>HTMLElement.dispatchEvent(new Event('dblclick'))</c> JavaScript.
    /// </summary>
    [Obsolete("Use " + nameof(DoubleClicksUsingScriptAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class DoubleClickUsingScriptAttribute : DoubleClicksUsingScriptAttribute
    {
    }
}
