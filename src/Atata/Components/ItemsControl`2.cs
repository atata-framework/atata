namespace Atata;

/// <summary>
/// Represents the items control (a control containing a set of any control of <typeparamref name="TItem"/> type).
/// Default search finds the first occurring element.
/// </summary>
/// <typeparam name="TItem">The type of the item control.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
[ControlDefinition(ComponentTypeName = "control", IgnoreNameEndings = "ItemsControl,Control")]
public class ItemsControl<TItem, TOwner> : Control<TOwner>
    where TItem : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the items' <see cref="ControlList{TItem, TOwner}"/> instance.
    /// </summary>
    public ControlList<TItem, TOwner> Items { get; private set; }

    /// <summary>
    /// Gets the item at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the item to get.</param>
    /// <returns>The child item at the specified index.</returns>
    public TItem this[int index] => Items[index];

    /// <summary>
    /// Gets the item that matches the conditions defined by the specified predicate expression.
    /// </summary>
    /// <param name="predicateExpression">The predicate expression to test each item.</param>
    /// <returns>The first child item that matches the conditions of the specified predicate.</returns>
    public TItem this[Expression<Func<TItem, bool>> predicateExpression] => Items[predicateExpression];
}
