using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by <see cref="IWebElement.Click()"/> method.
    /// </summary>
    [Obsolete("Use " + nameof(ClicksUsingClickMethodAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ClickUsingClickMethodAttribute : ClicksUsingClickMethodAttribute
    {
    }
}
