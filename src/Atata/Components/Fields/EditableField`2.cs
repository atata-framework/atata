﻿namespace Atata;

/// <summary>
/// Represents the base class for editable field controls.
/// It can be used for controls like <c>&lt;input&gt;</c>, <c>&lt;select&gt;</c> and other editable controls.
/// </summary>
/// <typeparam name="TValue">The type of the control's value.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public abstract class EditableField<TValue, TOwner> : Field<TValue, TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the value indicating whether the control is read-only.
    /// By default checks <c>readonly</c> attribute of scope element.
    /// Override <see cref="GetIsReadOnly"/> method to change the behavior.
    /// </summary>
    public ValueProvider<bool, TOwner> IsReadOnly =>
        CreateValueProvider("read-only state", GetIsReadOnly);

    /// <inheritdoc cref="UIComponent{TOwner}.Should"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, EditableField<TValue, TOwner>, TOwner> Should =>
        new(this);

    /// <inheritdoc cref="UIComponent{TOwner}.ExpectTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, EditableField<TValue, TOwner>, TOwner> ExpectTo =>
        Should.Using(ExpectationVerificationStrategy.Instance);

    /// <inheritdoc cref="UIComponent{TOwner}.WaitTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, EditableField<TValue, TOwner>, TOwner> WaitTo =>
        Should.Using(WaitingVerificationStrategy.Instance);

    protected virtual bool GetIsReadOnly() =>
        DomProperties.ReadOnly.Value == true;

    /// <summary>
    /// Converts the value to string for <see cref="SetValue(TValue)"/> method.
    /// Can use format from <see cref="ValueSetFormatAttribute"/>,
    /// otherwise from <see cref="FormatAttribute"/>.
    /// Can use culture from <see cref="CultureAttribute"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The value converted to string.</returns>
    protected virtual string? ConvertValueToStringUsingSetFormat(TValue value)
    {
        string? setFormat = Metadata.Get<ValueSetFormatAttribute>()?.Value;

        return setFormat is not null
            ? TermResolver.ToString(value, new TermOptions().MergeWith(GetValueTermOptions()).WithFormat(setFormat))
            : ConvertValueToString(value);
    }

    /// <summary>
    /// Sets the value.
    /// </summary>
    /// <param name="value">The value.</param>
    protected abstract void SetValue(TValue value);

    /// <summary>
    /// Sets the value.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner Set(TValue value)
    {
        ExecuteTriggers(TriggerEvents.BeforeSet);

        Log.ExecuteSection(
            new ValueSetLogSection(this, ConvertValueToString(value)),
            () => SetValue(value));

        ExecuteTriggers(TriggerEvents.AfterSet);

        return Owner;
    }

    /// <summary>
    /// Sets the random value.
    /// For value generation uses randomization attributes, for example:
    /// <see cref="RandomizeStringSettingsAttribute"/>, <see cref="RandomizeNumberSettingsAttribute"/>, <see cref="RandomizeIncludeAttribute"/>, etc.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner SetRandom()
    {
        TValue value = GenerateRandomValue();
        return Set(value);
    }

    /// <summary>
    /// Sets the random value and records it to <paramref name="value"/> parameter.
    /// For value generation uses randomization attributes, for example:
    /// <see cref="RandomizeStringSettingsAttribute" />, <see cref="RandomizeNumberSettingsAttribute" />, <see cref="RandomizeIncludeAttribute" />, etc.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <param name="value">The generated value.</param>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner SetRandom([NotNull] out TValue value)
    {
        value = GenerateRandomValue();
        return Set(value);
    }

    /// <summary>
    /// Sets the random value and invokes <paramref name="callback"/>.
    /// For value generation uses randomization attributes, for example:
    /// <see cref="RandomizeStringSettingsAttribute" />, <see cref="RandomizeNumberSettingsAttribute" />, <see cref="RandomizeIncludeAttribute" />, etc.
    /// Also executes <see cref="TriggerEvents.BeforeSet" /> and <see cref="TriggerEvents.AfterSet" /> triggers.
    /// </summary>
    /// <param name="callback">The callback to be invoked after the value is set.</param>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner SetRandom(Action<TValue> callback)
    {
        TValue value = GenerateRandomValue();
        Set(value);
        callback?.Invoke(value);
        return Owner;
    }

    /// <summary>
    /// Generates the random value.
    /// </summary>
    /// <returns>The generated value.</returns>
    [return: NotNull]
    protected virtual TValue GenerateRandomValue() =>
        ValueRandomizer.GetRandom<TValue>(Metadata);
}
