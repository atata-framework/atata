#nullable enable

namespace Atata;

/// <summary>
/// Represents the list of <see cref="Label{TOwner}"/> controls.
/// Gives opportunity to get a <see cref="Label{TOwner}"/> for particular control using its element <c>id</c> attribute that equals the <c>for</c> attribute of <c>&lt;label&gt;</c> element.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
/// <seealso cref="Label{TOwner}"/>
public class LabelList<TOwner> : AssociatedControlList<Label<TOwner>, TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Creates an instance of <see cref="Label{TOwner}"/> control that is associated with the <paramref name="control" /> by <c>for</c>/<c>id</c> attributes.
    /// </summary>
    /// <param name="control">The control for which the <see cref="Label{TOwner}"/> control is to be created.</param>
    /// <returns>The <see cref="Label{TOwner}"/> control.</returns>
    protected override Label<TOwner> CreateAssociatedControl(Control<TOwner> control)
    {
        string? id = control.DomProperties.Id;

        if (id is null or [])
            throw new InvalidOperationException($"The control element does not have an \"id\" attribute. Control: {control}");

        return Component.Find<Label<TOwner>>(control.ComponentName, new FindByAttributeAttribute("for", id));
    }
}
