namespace Atata
{
    /// <summary>
    /// Represents the inline frame control (&lt;iframe&gt;). Default search finds the first occurring &lt;iframe&gt; element.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlDefinition("iframe", ComponentTypeName = "iframe")]
    public class Frame<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Switches to the frame page object represented by the instance of <typeparamref name="TFramePageObject"/> type.
        /// </summary>
        /// <typeparam name="TFramePageObject">The type of the frame page object.</typeparam>
        /// <param name="framePageObject">The frame page object.</param>
        /// <param name="temporarily">If set to <c>true</c> navigates temporarily preserving current page object state.</param>
        /// <returns>The instance of the frame page object.</returns>
        public virtual TFramePageObject SwitchTo<TFramePageObject>(TFramePageObject framePageObject = null, bool? temporarily = null)
            where TFramePageObject : PageObject<TFramePageObject>
        {
            bool isTemporarily = temporarily
                ?? Metadata.Get<GoTemporarilyAttribute>(AttributeLevels.DeclaredAndComponent)?.IsTemporarily
                ?? false;

            return Owner.SwitchToFrame(Scope, framePageObject, isTemporarily);
        }
    }
}
