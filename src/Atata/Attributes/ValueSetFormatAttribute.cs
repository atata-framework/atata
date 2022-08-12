namespace Atata
{
    /// <summary>
    /// Specifies the data format to use for setting the value to the control.
    /// Can override control's <see cref="FormatAttribute"/>, but only for setting the value.
    /// </summary>
    public class ValueSetFormatAttribute : MulticastAttribute
    {
        public ValueSetFormatAttribute(string value) =>
            Value = value;

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Value { get; }
    }
}
