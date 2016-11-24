namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element's attribute values.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class UIComponentAttributeProvider<TOwner> : UIComponentPart<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public DataProvider<string, TOwner> Id
        {
            get { return GetAttibuteProvider(nameof(Id)); }
        }

        public DataProvider<string, TOwner> Name
        {
            get { return GetAttibuteProvider(nameof(Name)); }
        }

        public DataProvider<string, TOwner> Value
        {
            get { return GetAttibuteProvider(nameof(Value)); }
        }

        public DataProvider<string, TOwner> Title
        {
            get { return GetAttibuteProvider(nameof(Title)); }
        }

        public DataProvider<string, TOwner> Class
        {
            get { return GetAttibuteProvider(nameof(Class)); }
        }

        public DataProvider<string, TOwner> Href
        {
            get { return GetAttibuteProvider(nameof(Href)); }
        }

        public DataProvider<string, TOwner> For
        {
            get { return GetAttibuteProvider(nameof(For)); }
        }

        public DataProvider<string, TOwner> Type
        {
            get { return GetAttibuteProvider(nameof(Type)); }
        }

        public DataProvider<string, TOwner> Style
        {
            get { return GetAttibuteProvider(nameof(Style)); }
        }

        public DataProvider<string, TOwner> Alt
        {
            get { return GetAttibuteProvider(nameof(Alt)); }
        }

        public DataProvider<string, TOwner> Placeholder
        {
            get { return GetAttibuteProvider(nameof(Placeholder)); }
        }

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

        private DataProvider<string, TOwner> GetAttibuteProvider(string name)
        {
            string lowerCaseName = name.ToLower();
            return Component.GetOrCreateDataProvider(lowerCaseName, () => GetValue(lowerCaseName));
        }
    }
}
