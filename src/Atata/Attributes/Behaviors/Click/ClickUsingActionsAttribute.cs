using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control clicking by using a set of actions:
    /// <see cref="Actions.MoveToElement(IWebElement)"/> or <see cref="Actions.MoveToElement(IWebElement, int, int, MoveToElementOffsetOrigin)"/> and <see cref="Actions.Click()"/>.
    /// </summary>
    [Obsolete("Use " + nameof(ClicksUsingActionsAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class ClickUsingActionsAttribute : ClicksUsingActionsAttribute
    {
    }
}
