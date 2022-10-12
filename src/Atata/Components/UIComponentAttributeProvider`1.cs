using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's attribute values.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [Obsolete("Use UIComponentDomAttributesProvider<TOwner> or UIComponentDomPropertiesProvider<TOwner> class instead.")] // Obsolete since v2.3.0.
    public class UIComponentAttributeProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        private const string AttributeProviderNameFormat = "{0} attribute";

        [Obsolete("Use {component}.DomProperties.Id instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Id => Get<string>(nameof(Id));

        [Obsolete("Use {component}.DomProperties.Name instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Name => Get<string>(nameof(Name));

        [Obsolete("Use {component}.DomProperties.Value instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Value => Get<string>(nameof(Value));

        [Obsolete("Use {component}.DomProperties.Title instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Title => Get<string>(nameof(Title));

        [Obsolete("Use Href or HrefAttribue of Link<TOwner> class instead. Alternatively use {component}.DomProperties[\"href\"] or {component}.DomAttributes[\"href\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Href => Get<string>(nameof(Href));

        [Obsolete("Use For of Label<TValue, TOwner> class instead. Alternatively use {component}.DomAttributes[\"for\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> For => Get<string>(nameof(For));

        [Obsolete("Use {component}.DomProperties[\"type\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Type => Get<string>(nameof(Type));

        [Obsolete("Use {component}.DomProperties.Style instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Style => Get<string>(nameof(Style));

        [Obsolete("Use Alt of Image<TOwner>/ImageInput<TOwner> class instead. Alternatively use {component}.DomProperties[\"alt\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Alt => Get<string>(nameof(Alt));

        [Obsolete("Use Placeholder of Input<TValue, TOwner>/TextArea<TOwner> class instead. Alternatively use {component}.DomProperties[\"placeholder\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Placeholder => Get<string>(nameof(Placeholder));

        [Obsolete("Use {component}.DomProperties[\"target\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Target => Get<string>(nameof(Target));

        [Obsolete("Use Pattern of Input<TValue, TOwner> class instead. Alternatively use {component}.DomProperties[\"pattern\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Pattern => Get<string>(nameof(Pattern));

        [Obsolete("Use Accept of FileInput<TOwner> class instead. Alternatively use {component}.DomProperties[\"accept\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Accept => Get<string>(nameof(Accept));

        [Obsolete("Use Source or SourceAttribue of Image<TOwner>/ImageInput<TOwner> class instead. Alternatively use {component}.DomProperties[\"src\"] or {component}.DomAttributes[\"src\"] instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> Src => Get<string>(nameof(Src));

        [Obsolete("Use {component}.DomProperties.TextContent instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> TextContent => Component.CreateValueProvider(
            AttributeProviderNameFormat.FormatWith("textContent"),
            () => GetValue("textContent")?.Trim());

        [Obsolete("Use {component}.DomProperties.InnerHtml instead.")] // Obsolete since v2.3.0.
        public ValueProvider<string, TOwner> InnerHtml => Component.CreateValueProvider(
            AttributeProviderNameFormat.FormatWith("innerHTML"),
            () => GetValue("innerHTML")?.Trim());

        [Obsolete("Use {component}.DomProperties.Get<bool?>(\"disabled\") instead.")] // Obsolete since v2.3.0.
        public ValueProvider<bool, TOwner> Disabled => Get<bool>(nameof(Disabled));

        [Obsolete("Use {component}.DomProperties.ReadOnly instead.")] // Obsolete since v2.3.0.
        public ValueProvider<bool, TOwner> ReadOnly => Get<bool>(nameof(ReadOnly));

        [Obsolete("Use {component}.DomProperties.Get<bool?>(\"checked\") instead.")] // Obsolete since v2.3.0.
        public ValueProvider<bool, TOwner> Checked => Get<bool>(nameof(Checked));

        [Obsolete("Use {component}.DomProperties.Required instead.")] // Obsolete since v2.3.0.
        public ValueProvider<bool, TOwner> Required => Get<bool>(nameof(Required));

        [Obsolete("Use {component}.DomClasses instead.")] // Obsolete since v2.3.0.
        public ValueProvider<IEnumerable<string>, TOwner> Class => Component.CreateValueProvider<IEnumerable<string>>(
            "class attribute",
            () => GetValue("class").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));

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

            string lowerCaseName = attributeName.ToLowerInvariant();
            return Component.CreateValueProvider(AttributeProviderNameFormat.FormatWith(lowerCaseName), () => GetValue<TValue>(lowerCaseName));
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

            return Component.Scope.GetAttribute(attributeName);
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
