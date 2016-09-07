namespace Atata
{
    /// <summary>
    /// Allows to access the component scope element CSS property values.
    /// </summary>
    public class UIComponentCssProvider
    {
        private readonly UIComponent component;

        public UIComponentCssProvider(UIComponent component)
        {
            this.component = component;
        }

        /// <summary>
        /// Gets the value of the specified CSS property for the control's scope element.
        /// </summary>
        /// <param name="propertyName">The name of the CSS property.</param>
        /// <returns>The value of the specified CSS property.</returns>
        public string this[string propertyName]
        {
            get { return component.Scope.GetCssValue(propertyName); }
        }
    }
}
