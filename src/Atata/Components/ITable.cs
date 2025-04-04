namespace Atata;

/// <summary>
/// Represents an interface of table control.
/// </summary>
internal interface ITable
{
    /// <summary>
    /// Gets the column header texts.
    /// </summary>
    /// <returns>The collection of text values.</returns>
    IEnumerable<string> GetColumnHeaderTexts();
}
