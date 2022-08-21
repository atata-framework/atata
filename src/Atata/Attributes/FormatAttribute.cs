namespace Atata
{
    /// <summary>
    /// Specifies the data format of the control.
    /// Format value is similar to format of <see cref="string.Format(string, object[])"/> method.
    /// For example: <c>"start {0} end"</c>, <c>"{0:C2}"</c>, <c>"{0:MM/dd/yyyy}"</c>.
    /// Note that <c>"{"</c> and <c>"}"</c> characters should be written as <c>"{{"</c> and <c>"}}"</c>.
    /// </summary>
    public class FormatAttribute : MulticastAttribute
    {
        public FormatAttribute(string value) =>
            Value = value;

        /// <summary>
        /// Gets the format value.
        /// </summary>
        public string Value { get; }
    }
}
