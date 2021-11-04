namespace Atata
{
    /// <summary>
    /// Represents an event that occurs when <see cref="PageObject{TOwner}"/> is de-initialized.
    /// </summary>
    public class PageObjectDeInitEvent
    {
        public PageObjectDeInitEvent(UIComponent pageObject)
        {
            PageObject = pageObject;
        }

        /// <summary>
        /// Gets the page object.
        /// </summary>
        public UIComponent PageObject { get; }
    }
}
