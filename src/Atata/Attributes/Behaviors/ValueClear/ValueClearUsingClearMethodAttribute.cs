using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by <see cref="IWebElement.Clear()"/> method.
    /// </summary>
    [Obsolete("Use " + nameof(ClearsValueUsingClearMethodAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueClearUsingClearMethodAttribute : ClearsValueUsingClearMethodAttribute
    {
    }
}
