namespace Atata
{
    /// <summary>
    /// Specifies the data format of the control.
    /// </summary>
    public class FormatAttribute : MulticastAttribute
    {
        public FormatAttribute(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Value { get; private set; }
    }
}
