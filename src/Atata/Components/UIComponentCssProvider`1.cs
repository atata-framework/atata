namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's CSS property values.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentCssProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value of the specified control's scope element CSS property.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property.</param>
        /// <returns>The <see cref="DataProvider{TData, TOwner}"/> instance for the CSS property's current value.</returns>
        public DataProvider<string, TOwner> this[string propertyName]
        {
            get { return Component.GetOrCreateDataProvider(propertyName + "CSS property", () => GetValue(propertyName)); }
        }

        /// <summary>
        /// Gets the value of the specified control's scope element CSS property.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property.</param>
        /// <returns>The CSS property's current value.</returns>
        public string GetValue(string propertyName)
        {
            return Component.Scope.GetCssValue(propertyName);
        }
    }
}
