using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control right-clicking by executing <c>HTMLElement.dispatchEvent(new Event('contextmenu'))</c> JavaScript.
    /// </summary>
    [Obsolete("Use " + nameof(RightClicksUsingScriptAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class RightClickUsingScriptAttribute : RightClicksUsingScriptAttribute
    {
    }
}
