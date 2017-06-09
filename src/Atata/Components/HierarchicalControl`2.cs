using System;
using System.Linq.Expressions;

namespace Atata
{
    /// <summary>
    /// Represents the hierarchical control (a control containing structured hierarchy of controls of <typeparamref name="TItem"/> type). Default search finds the first occurring element.
    /// </summary>
    /// <typeparam name="TItem">The type of the item control.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public class HierarchicalControl<TItem, TOwner> : Control<TOwner>
        where TItem : HierarchicalItem<TItem, TOwner>
        where TOwner : PageObject<TOwner>
    {
        /// <summary>
        /// Gets the children <see cref="ControlList{TItem, TOwner}"/> instance.
        /// </summary>
        [FindSettings(OuterXPath = "./")]
        public ControlList<TItem, TOwner> Children { get; private set; }

        /// <summary>
        /// Gets the descendants (all items at any level of hierarchy) <see cref="ControlList{TItem, TOwner}"/> instance.
        /// </summary>
        [FindSettings(OuterXPath = ".//")]
        public ControlList<TItem, TOwner> Descendants { get; private set; }

        /// <summary>
        /// Gets the child item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to get.</param>
        /// <returns>The child item at the specified index.</returns>
        public TItem this[int index] => Children[index];

        /// <summary>
        /// Gets the child item that matches the conditions defined by the specified predicate expression.
        /// </summary>
        /// <param name="predicateExpression">The predicate expression to test each item.</param>
        /// <returns>The first child item that matches the conditions of the specified predicate.</returns>
        public TItem this[Expression<Func<TItem, bool>> predicateExpression] => Children[predicateExpression];
    }
}
