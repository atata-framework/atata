using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for drag and drop using WebDriver's <see cref="Actions"/>.
    /// Performs <see cref="Actions.ClickAndHold(IWebElement)"/>, <see cref="Actions.MoveToElement(IWebElement)"/> and <see cref="Actions.Release(IWebElement)"/> actions.
    /// </summary>
    [Obsolete("Use " + nameof(DragsAndDropsUsingActionsAttribute) + " instead.")] // Obsolete since v1.12.0.
    public class DragAndDropUsingActionsAttribute : DragsAndDropsUsingActionsAttribute
    {
    }
}
