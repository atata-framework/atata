using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value set by <see cref="IWebElement.SendKeys(string)"/> method.
    /// <see cref="IWebElement.SendKeys(string)"/> method is invoked only when the value is not null or empty.
    /// </summary>
    [Obsolete("Use " + nameof(SetsValueUsingSendKeysAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ValueSetUsingSendKeysAttribute : SetsValueUsingSendKeysAttribute
    {
    }
}
