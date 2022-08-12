namespace Atata
{
    /// <summary>
    /// Specifies the data format to use for getting the value of the control.
    /// Can override control's <see cref="FormatAttribute"/>, but only for getting the value.
    /// </summary>
    public class ValueGetFormatAttribute : MulticastAttribute
    {
        public ValueGetFormatAttribute(string value) =>
            Value = value;

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Value { get; }
    }
}
