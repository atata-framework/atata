namespace Atata
{
    /// <summary>
    /// Specifies the definition of the control, like scope XPath, visibility, component type name, etc.
    /// </summary>
    public class ControlDefinitionAttribute : UIComponentDefinitionAttribute, IHasOptionalProperties
    {
        public ControlDefinitionAttribute(string scopeXPath = DefaultScopeXPath)
            : base(scopeXPath)
        {
        }

        PropertyBag IHasOptionalProperties.OptionalProperties => OptionalProperties;

        protected PropertyBag OptionalProperties { get; } = new PropertyBag();

        /// <summary>
        /// Gets or sets the visibility.
        /// The default value is <see cref="Visibility.Visible"/>.
        /// </summary>
        public Visibility Visibility
        {
            get { return OptionalProperties.GetOrDefault(nameof(Visibility), Visibility.Visible); }
            set { OptionalProperties[nameof(Visibility)] = value; }
        }
    }
}
