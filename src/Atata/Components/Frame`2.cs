using System;

namespace Atata
{
    /// <summary>
    /// Represents the frame control (<c>&lt;iframe&gt;</c> or <c>&lt;frame&gt;</c>).
    /// Default search finds the first occurring <c>&lt;iframe&gt;</c> or <c>&lt;frame&gt;</c> element.
    /// </summary>
    /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Frame<TFramePageObject, TOwner> : Frame<TOwner>
        where TFramePageObject : PageObject<TFramePageObject>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Switches to the frame page object represented by the instance of <typeparamref name="TFramePageObject"/> type.
        /// </summary>
        /// <param name="framePageObject">The frame page object.</param>
        /// <param name="temporarily">
        /// If set to <see langword="true"/> navigates temporarily preserving current page object state.
        /// If is not set, checks <see cref="GoTemporarilyAttribute"/>.</param>
        /// <returns>The instance of the frame page object.</returns>
        public TFramePageObject SwitchTo(TFramePageObject framePageObject = null, bool? temporarily = null)
        {
            return SwitchTo<TFramePageObject>(framePageObject, temporarily);
        }

        /// <summary>
        /// Switches to the frame page object, executes action(s) in scope of frame and switches back to the owner page object.
        /// </summary>
        /// <param name="action">The action to do in scope of frame.</param>
        /// <param name="temporarily">
        /// If set to <see langword="true"/> navigates temporarily preserving current page object state.
        /// If is not set, checks <see cref="GoTemporarilyAttribute"/>.</param>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner DoWithin(Action<TFramePageObject> action, bool? temporarily = null)
        {
            return DoWithin<TFramePageObject>(action, temporarily);
        }
    }
}
