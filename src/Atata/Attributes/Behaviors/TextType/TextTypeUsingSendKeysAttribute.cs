using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control text typing by <see cref="IWebElement.SendKeys(string)"/> method.
    /// <see cref="IWebElement.SendKeys(string)"/> method is invoked only when the value is not null or empty.
    /// </summary>
    [Obsolete("Use " + nameof(TypesTextUsingSendKeysAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class TextTypeUsingSendKeysAttribute : TypesTextUsingSendKeysAttribute
    {
    }
}
