namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's DOM attribute values.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public sealed class UIComponentDomAttributesProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string AttributeProviderNameFormat = "\"{0}\" DOM attribute";

        public UIComponentDomAttributesProvider(IUIComponent<TOwner> component)
        {
            Component = component.CheckNotNull(nameof(component));
            ComponentPartName = "DOM attributes";
        }

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> instance for the value of the specified control's scope element attribute.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> instance for the attribute's current value.</returns>
        public ValueProvider<string, TOwner> this[string attributeName] =>
            Get<string>(attributeName);

        /// <summary>
        /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> instance for the value of the specified control's scope element attribute.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute value.</typeparam>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> instance for the attribute's current value.</returns>
        public ValueProvider<TValue, TOwner> Get<TValue>(string attributeName)
        {
            attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

            return Component.CreateValueProvider(
                AttributeProviderNameFormat.FormatWith(attributeName),
                () => GetValue<TValue>(attributeName));
        }

        /// <summary>
        /// Gets the value of the specified control's scope element attribute.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute's current value.
        /// Returns <see langword="null"/> if the value is not set.</returns>
        public string GetValue(string attributeName)
        {
            attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

            return Component.Scope.GetDomAttribute(attributeName);
        }

        /// <summary>
        /// Gets the value of the specified control's scope element attribute.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute value.</typeparam>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute's current value.
        /// Returns <see langword="null"/> if the value is not set.</returns>
        public TValue GetValue<TValue>(string attributeName)
        {
            string valueAsString = GetValue(attributeName);

            if (string.IsNullOrEmpty(valueAsString) && typeof(TValue) == typeof(bool))
                return default;

            return TermResolver.FromString<TValue>(valueAsString);
        }
    }
}
