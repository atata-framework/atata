using System;
using System.Collections.Generic;

namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's attribute values.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentAttributeProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public DataProvider<string, TOwner> Id => Get<string>(nameof(Id));

        public DataProvider<string, TOwner> Name => Get<string>(nameof(Name));

        public DataProvider<string, TOwner> Value => Get<string>(nameof(Value));

        public DataProvider<string, TOwner> Title => Get<string>(nameof(Title));

        public DataProvider<string, TOwner> Href => Get<string>(nameof(Href));

        public DataProvider<string, TOwner> For => Get<string>(nameof(For));

        public DataProvider<string, TOwner> Type => Get<string>(nameof(Type));

        public DataProvider<string, TOwner> Style => Get<string>(nameof(Style));

        public DataProvider<string, TOwner> Alt => Get<string>(nameof(Alt));

        public DataProvider<string, TOwner> Placeholder => Get<string>(nameof(Placeholder));

        public DataProvider<bool, TOwner> Disabled => Get<bool>(nameof(Disabled));

        public DataProvider<bool, TOwner> ReadOnly => Get<bool>(nameof(ReadOnly));

        public DataProvider<IEnumerable<string>, TOwner> Class => Component.GetOrCreateDataProvider<IEnumerable<string>>(
            "class attribute",
            () => GetValue("class").Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries));

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value of the specified control's scope element attribute.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The <see cref="DataProvider{TData, TOwner}"/> instance for the attribute's current value.</returns>
        public DataProvider<string, TOwner> this[string attributeName]
        {
            get { return Get<string>(attributeName); }
        }

        /// <summary>
        /// Gets the <see cref="DataProvider{TData, TOwner}"/> instance for the value of the specified control's scope element attribute.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute value.</typeparam>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The <see cref="DataProvider{TData, TOwner}"/> instance for the attribute's current value.</returns>
        public DataProvider<TValue, TOwner> Get<TValue>(string attributeName)
        {
            string lowerCaseName = attributeName.ToLower();
            return Component.GetOrCreateDataProvider($"{lowerCaseName} attribute", () => GetValue<TValue>(lowerCaseName));
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

        /// <summary>
        /// Gets the value of the specified control's scope element attribute.
        /// </summary>
        /// <typeparam name="TValue">The type of the attribute value.</typeparam>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute's current value. Returns a null if the value is not set.</returns>
        public TValue GetValue<TValue>(string attributeName)
        {
            string valueAsString = Component.Scope.GetAttribute(attributeName);

            if (string.IsNullOrEmpty(valueAsString) && typeof(TValue) == typeof(bool))
                return default(TValue);

            return TermResolver.FromString<TValue>(valueAsString);
        }
    }
}
