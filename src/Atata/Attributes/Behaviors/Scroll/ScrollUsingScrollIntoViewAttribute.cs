using System;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for scrolling to control using JavaScript.
    /// Performs <c>element.scrollIntoView()</c> function.
    /// </summary>
    [Obsolete("Use " + nameof(ScrollsUsingScriptAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ScrollUsingScrollIntoViewAttribute : ScrollsUsingScriptAttribute
    {
    }
}
