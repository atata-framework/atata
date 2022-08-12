namespace Atata
{
    /// <summary>
    /// Specifies the name of the component.
    /// </summary>
    public class NameAttribute : MulticastAttribute
    {
        public NameAttribute(string value) =>
            Value = value;

        /// <summary>
        /// Gets the name value.
        /// </summary>
        public string Value { get; }
    }
}
