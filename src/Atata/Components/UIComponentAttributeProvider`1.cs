namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's attribute values.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentAttributeProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value of the specified control's scope element attribute.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The <see cref="DataProvider{TData, TOwner}"/> instance for the attribute's current value.</returns>
        public DataProvider<string, TOwner> this[string attributeName]
        {
            get { return Component.GetOrCreateDataProvider(attributeName, () => GetValue(attributeName)); }
        }

        /// <summary>
        /// Gets the value of the specified control's scope element attribute.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute's current value. Returns a null if the value is not set.</returns>
        public string GetValue(string attributeName)
        {
            return Component.Scope.GetAttribute(attributeName);
        }
    }
}
