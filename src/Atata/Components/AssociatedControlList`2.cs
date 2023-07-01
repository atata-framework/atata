namespace Atata;

/// <summary>
/// Represents the list of associated controls of <typeparamref name="TItem"/> type.
/// Provides functionality to get associated/dependent <typeparamref name="TItem"/> control to another control.
/// </summary>
/// <typeparam name="TItem">The type of the item control.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public abstract class AssociatedControlList<TItem, TOwner> : ControlList<TItem, TOwner>
    where TItem : Control<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <typeparamref name="TItem"/> control associated with the control specified in <paramref name="controlSelector"/>.
    /// </summary>
    /// <param name="controlSelector">The control selector.</param>
    /// <returns>The <typeparamref name="TItem"/> control associated with the control specified in <paramref name="controlSelector"/>.</returns>
    public TItem this[Func<TOwner, Control<TOwner>> controlSelector]
        => For(controlSelector);

    /// <summary>
    /// Gets the <typeparamref name="TItem"/> control associated with the control specified in <paramref name="controlSelector"/>.
    /// </summary>
    /// <param name="controlSelector">The control selector.</param>
    /// <returns>The <typeparamref name="TItem"/> control associated with the control specified in <paramref name="controlSelector"/>.</returns>
    public TItem For(Func<TOwner, Control<TOwner>> controlSelector)
    {
        Control<TOwner> targetControl = controlSelector(Component.Owner);

        return CreateAssociatedControl(targetControl);
    }

    /// <summary>
    /// Creates an instance of <typeparamref name="TItem"/> control that is associated with the <paramref name="control"/> argument.
    /// </summary>
    /// <param name="control">The control for which the associated control is to be created.</param>
    /// <returns>The <typeparamref name="TItem"/> control.</returns>
    protected abstract TItem CreateAssociatedControl(Control<TOwner> control);
}
