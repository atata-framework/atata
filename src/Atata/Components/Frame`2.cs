namespace Atata
{
    /// <summary>
    /// Represents the inline frame control (&lt;iframe&gt;). Default search finds the first occurring &lt;iframe&gt; element.
    /// </summary>
    /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class Frame<TFramePageObject, TOwner> : Frame<TOwner>
        where TOwner : PageObject<TOwner>
        where TFramePageObject : PageObject<TFramePageObject>
    {
        /// <summary>
        /// Switches to the frame page object represented by the instance of <typeparamref name="TFramePageObject"/> type.
        /// </summary>
        /// <param name="framePageObject">The frame page object.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The instance of the frame page object.</returns>
        public TFramePageObject SwitchTo(TFramePageObject framePageObject = null, bool? temporarily = null)
        {
            return SwitchTo<TFramePageObject>(framePageObject, temporarily);
        }
    }
}
