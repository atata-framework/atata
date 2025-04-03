#nullable enable

namespace Atata;

/// <summary>
/// Represents a strategy that finds a control in a cell searched by the specified column index.
/// </summary>
public class FindByColumnIndexStrategy : XPathComponentScopeFindStrategy
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FindByColumnIndexStrategy"/> class.
    /// </summary>
    /// <param name="columnIndex">Index of the column.</param>
    public FindByColumnIndexStrategy(int columnIndex) =>
        ColumnIndex = columnIndex;

    /// <summary>
    /// Gets the index of the column.
    /// </summary>
    public int ColumnIndex { get; }

    /// <summary>
    /// Gets or sets the XPath of the cell.
    /// The default value is <c>"td"</c>.
    /// </summary>
    public string CellXPath { get; set; } = "td";

    protected override string Build(ComponentScopeXPathBuilder builder, ComponentScopeFindOptions options) =>
        builder.WrapWithIndex(x => x.OuterXPath._(CellXPath).WhereIndex(ColumnIndex).DescendantOrSelf.ComponentXPath);
}
