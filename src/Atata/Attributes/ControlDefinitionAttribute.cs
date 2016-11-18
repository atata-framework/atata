namespace Atata
{
    public class ControlDefinitionAttribute : UIComponentDefinitionAttribute
    {
        public ControlDefinitionAttribute(string scopeXPath = null)
            : base(scopeXPath)
        {
        }

        /// <summary>
        /// Gets or sets the visibility. The default value is Visible.
        /// </summary>
        public Visibility Visibility
        {
            get { return Properties.Get(nameof(Visibility), Visibility.Visible); }
            set { Properties[nameof(Visibility)] = value; }
        }
    }
}
