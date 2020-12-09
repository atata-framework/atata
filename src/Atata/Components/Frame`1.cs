using System;

namespace Atata
{
    /// <summary>
    /// Represents the frame control (<c>&lt;iframe&gt;</c> or <c>&lt;frame&gt;</c>).
    /// Default search finds the first occurring <c>&lt;iframe&gt;</c> or <c>&lt;frame&gt;</c> element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("*[self::iframe or self::frame]", ComponentTypeName = "frame")]
    public class Frame<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Switches to the frame page object represented by the instance of <typeparamref name="TFramePageObject"/> type.
        /// </summary>
        /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
        /// <param name="framePageObject">The frame page object.</param>
        /// <param name="temporarily">
        /// If set to <see langword="true"/> navigates temporarily preserving current page object state.
        /// If is not set, checks <see cref="GoTemporarilyAttribute"/>.</param>
        /// <returns>The instance of the frame page object.</returns>
        public virtual TFramePageObject SwitchTo<TFramePageObject>(TFramePageObject framePageObject = null, bool? temporarily = null)
            where TFramePageObject : PageObject<TFramePageObject>
        {
            bool isTemporarily = temporarily
                ?? Metadata.Get<GoTemporarilyAttribute>()?.IsTemporarily
                ?? false;

            Owner.SwitchToFrame(this);

            return Go.To(framePageObject, navigate: false, temporarily: isTemporarily);
        }

        /// <summary>
        /// Switches to the frame page object, executes action(s) in scope of frame and switches back to the owner page object.
        /// </summary>
        /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
        /// <param name="action">The action to do in scope of frame.</param>
        /// <param name="temporarily">
        /// If set to <see langword="true"/> navigates temporarily preserving current page object state.
        /// If is not set, checks <see cref="GoTemporarilyAttribute"/>.</param>
        /// <returns>The instance of the owner page object.</returns>
        public TOwner DoWithin<TFramePageObject>(Action<TFramePageObject> action, bool? temporarily = null)
            where TFramePageObject : PageObject<TFramePageObject>
        {
            action.CheckNotNull(nameof(action));

            TFramePageObject framePageObject = SwitchTo<TFramePageObject>(temporarily: temporarily);

            action(framePageObject);

            return ((IPageObject)AtataContext.Current.PageObject).SwitchToRoot(Owner);
        }
    }
}
