using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by trying to execute <see cref="IWebElement.Clear()"/> method.
    /// If <see cref="InvalidElementStateException"/> occurs, then clears the value by executing
    /// <c>HTMLElement.value = ''; HTMLElement.dispatchEvent(new Event('change'));</c> JavaScript.
    /// </summary>
    [Obsolete("Use " + nameof(ClearsValueUsingClearMethodOrScriptAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueClearUsingClearMethodOrScriptAttribute : ClearsValueUsingClearMethodOrScriptAttribute
    {
    }
}
