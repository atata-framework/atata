namespace Atata
{
    /// <summary>
    /// Specifies the definition of the control, like scope XPath, visibility, component type name, etc.
    /// </summary>
    public class ControlDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public ControlDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
            : base(scopeXPath)
        {
        }

        /// <summary>
        /// Gets or sets the visibility.
        /// The default value is <see cref="Visibility.Visible"/>.
        /// </summary>
        public Visibility Visibility
        {
            get { return Properties.Get(nameof(Visibility), Visibility.Visible); }
            set { Properties[nameof(Visibility)] = value; }
        }
    }
}
