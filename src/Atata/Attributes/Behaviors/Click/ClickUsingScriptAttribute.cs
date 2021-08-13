using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by executing <c>HTMLElement.click()</c> JavaScript method.
    /// </summary>
    [Obsolete("Use " + nameof(ClicksUsingScriptAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ClickUsingScriptAttribute : ClicksUsingScriptAttribute
    {
    }
}
