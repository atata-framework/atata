using System.Runtime.InteropServices;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Represents the behavior for control value clearing by performing "Ctrl+A, Delete" ("Cmd+A, Delete" on macOS) keyboard shortcut.
    /// </summary>
    public class ClearsValueUsingCtrlADeleteKeysAttribute : ValueClearBehaviorAttribute
    {
        /// <inheritdoc/>
        public override void Execute<TOwner>(IUIComponent<TOwner> component)
        {
            var scopeElement = component.Scope;

            var platformSpecificKey = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? Keys.Command : Keys.Control;

            component.Owner.Driver.Perform(x => x
                .KeyDown(scopeElement, platformSpecificKey)
                .SendKeys("a")
                .KeyUp(platformSpecificKey)
                .SendKeys(Keys.Delete));
        }
    }
}
