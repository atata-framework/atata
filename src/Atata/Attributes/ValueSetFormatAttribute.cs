namespace Atata
{
    /// <summary>
    /// Specifies the data format to use for setting the value to the control.
    /// Can override control's <see cref="FormatAttribute"/>, but only for setting the value.
    /// Format value is similar to format of <see cref="string.Format(string, object[])"/> method.
    /// For example: <c>"start {0} end"</c>, <c>"{0:C2}"</c>, <c>"{0:MM/dd/yyyy}"</c>.
    /// Note that <c>"{"</c> and <c>"}"</c> characters should be written as <c>"{{"</c> and <c>"}}"</c>.
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
