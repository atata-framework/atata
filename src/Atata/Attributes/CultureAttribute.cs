namespace Atata
{
    /// <summary>
    /// Specifies the culture of the component.
    /// </summary>
    public class CultureAttribute : MulticastAttribute
    {
        public CultureAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the culture value.
        /// </summary>
        public string Value { get; }
    }
}
