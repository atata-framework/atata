using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by invoking
    /// <see cref="IWebElement.Clear()"/> and <see cref="IWebElement.SendKeys(string)"/> methods.
    /// <see cref="IWebElement.SendKeys(string)"/> method is invoked only when the value is not null or empty.
    /// </summary>
    [Obsolete("Use " + nameof(SetsValueUsingClearAndSendKeysAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueSetUsingClearAndSendKeysAttribute : SetsValueUsingClearAndSendKeysAttribute
    {
    }
}
