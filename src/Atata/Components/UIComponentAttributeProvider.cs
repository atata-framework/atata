namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element attribute values.
    /// </summary>
    public class UIComponentAttributeProvider
    {
        private readonly UIComponent component;

        public UIComponentAttributeProvider(UIComponent component)
        {
            this.component = component;
        }

        /// <summary>
        /// Gets the value of the specified attribute for the control's scope element.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns>The attribute's current value. Returns a null if the value is not set.</returns>
        public string this[string attributeName]
        {
            get { return component.Scope.GetAttribute(attributeName); }
        }
    }
}
