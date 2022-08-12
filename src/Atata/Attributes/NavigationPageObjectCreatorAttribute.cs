using System;

namespace Atata
{
    /// <summary>
    /// Specifies the function that creates a page object for navigation.
    /// </summary>
    public class NavigationPageObjectCreatorAttribute : MulticastAttribute
    {
        public NavigationPageObjectCreatorAttribute(Func<object> creator) =>
            Creator = creator;

        /// <summary>
        /// Gets the creator function.
        /// </summary>
        public Func<object> Creator { get; }
    }
}
