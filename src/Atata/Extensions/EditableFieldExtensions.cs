namespace Atata;

public static class EditableFieldExtensions
{
    /// <summary>
    /// Sets the random value and records it to <paramref name="value"/> parameter.
    /// For value generation uses randomization attributes, for example:
    /// <see cref="RandomizeStringSettingsAttribute" />, <see cref="RandomizeNumberSettingsAttribute" />, <see cref="RandomizeIncludeAttribute" />, etc.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="field">The editable field control.</param>
    /// <param name="value">The generated value.</param>
    /// <returns>The instance of the owner page object.</returns>
    public static TOwner SetRandom<TOwner>(this EditableField<decimal?, TOwner> field, out int? value)
        where TOwner : PageObject<TOwner>
    {
        field.SetRandom(out decimal? decimalValue);

        value = (int?)decimalValue;

        return field.Owner;
    }

    /// <summary>
    /// Sets the random value and records it to <paramref name="value"/> parameter.
    /// For value generation uses randomization attributes, for example:
    /// <see cref="RandomizeStringSettingsAttribute" />, <see cref="RandomizeNumberSettingsAttribute" />, <see cref="RandomizeIncludeAttribute" />, etc.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="field">The editable field control.</param>
    /// <param name="value">The generated value.</param>
    /// <returns>The instance of the owner page object.</returns>
    public static TOwner SetRandom<TOwner>(this EditableField<decimal?, TOwner> field, out int value)
        where TOwner : PageObject<TOwner>
    {
        field.SetRandom(out decimal? decimalValue);

        value = (int)decimalValue;

        return field.Owner;
    }

    /// <summary>
    /// Sets the random value and records it to <paramref name="value"/> parameter.
    /// For value generation uses randomization attributes, for example:
    /// <see cref="RandomizeStringSettingsAttribute" />, <see cref="RandomizeNumberSettingsAttribute" />, <see cref="RandomizeIncludeAttribute" />, etc.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <typeparam name="TData">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    /// <param name="field">The editable field control.</param>
    /// <param name="value">The generated value.</param>
    /// <returns>The instance of the owner page object.</returns>
    public static TOwner SetRandom<TData, TOwner>(this EditableField<TData?, TOwner> field, out TData value)
        where TData : struct
        where TOwner : PageObject<TOwner>
    {
        field.SetRandom(out TData? nullableValue);

        value = (TData)nullableValue;

        return field.Owner;
    }
}
