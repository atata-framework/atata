namespace Atata
{
    public static class INavigableExtensions
    {
        /// <summary>
        /// Clicks the control and performs the navigation to the page object of <typeparamref name="TNavigateTo"/> type.
        /// Also executes <see cref="TriggerEvents.BeforeClick" /> and <see cref="TriggerEvents.AfterClick" /> triggers.
        /// </summary>
        /// <typeparam name="TNavigateTo">The type of the page object to navigate to.</typeparam>
        /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
        /// <param name="navigableControl">The navigable control.</param>
        /// <returns>The instance of <typeparamref name="TNavigateTo"/>.</returns>
        public static TNavigateTo ClickAndGo<TNavigateTo, TOwner>(this INavigable<TNavigateTo, TOwner> navigableControl)
            where TNavigateTo : PageObject<TNavigateTo>
            where TOwner : PageObject<TOwner>
        {
            return navigableControl.ClickAndGo<TNavigateTo>();
        }
    }
}
