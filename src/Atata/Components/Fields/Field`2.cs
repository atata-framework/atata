﻿namespace Atata;

/// <summary>
/// Represents the base class for the field controls.
/// It can be used for HTML elements containing content
/// (like <c>&lt;h1&gt;</c>, <c>&lt;span&gt;</c>, etc.) representing content as a field value,
/// as well as for <c>&lt;input&gt;</c> and other elements.
/// </summary>
/// <typeparam name="TValue">The type of the control's value.</typeparam>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public abstract class Field<TValue, TOwner> : Control<TOwner>, IEquatable<TValue>, IObjectProvider<TValue, TOwner>, IConvertsValueToString<TValue>
    where TOwner : PageObject<TOwner>
{
    protected TValue? CachedValue { get; private set; }

    [MemberNotNullWhen(true, nameof(CachedValue))]
    protected bool HasCachedValue { get; private set; }

    protected bool UsesValueCache =>
        Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute or UsesValueCacheAttribute))
            ?.UsesCache ?? false;

    TValue IObjectProvider<TValue>.Object => Get();

    /// <summary>
    /// Gets the value.
    /// Also executes <see cref="TriggerEvents.BeforeGet"/> and <see cref="TriggerEvents.AfterGet"/> triggers.
    /// </summary>
    public TValue Value => Get();

    /// <summary>
    /// Gets the name of the value provider.
    /// The default value is <c>"value"</c>.
    /// </summary>
    protected virtual string ValueProviderName => "value";

    string IObjectProvider<TValue>.ProviderName =>
        BuildFullValueProviderName(ValueProviderName);

    TOwner IObjectProvider<TValue, TOwner>.Owner => Owner;

    bool IObjectProvider<TValue, TOwner>.IsDynamic => true;

    IAtataExecutionUnit IObjectProvider<TValue>.ExecutionUnit =>
        Session.ExecutionUnit;

    /// <inheritdoc cref="UIComponent{TOwner}.Should"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, Field<TValue, TOwner>, TOwner> Should =>
        new(this);

    /// <inheritdoc cref="UIComponent{TOwner}.ExpectTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, Field<TValue, TOwner>, TOwner> ExpectTo =>
        Should.Using(ExpectationVerificationStrategy.Instance);

    /// <inheritdoc cref="UIComponent{TOwner}.WaitTo"/>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    public new FieldVerificationProvider<TValue, Field<TValue, TOwner>, TOwner> WaitTo =>
        Should.Using(WaitingVerificationStrategy.Instance);

    public static explicit operator TValue(Field<TValue, TOwner> field) =>
        field.Get();

    public static bool operator ==(Field<TValue, TOwner> field, TValue value) =>
        field is null
            ? Equals(value, null)
            : field.Equals(value);

    public static bool operator !=(Field<TValue, TOwner> field, TValue value) =>
        !(field == value);

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <returns>The value.</returns>
    protected abstract TValue GetValue();

    /// <summary>
    /// Gets the value. Also executes <see cref="TriggerEvents.BeforeGet"/> and <see cref="TriggerEvents.AfterGet"/> triggers.
    /// </summary>
    /// <returns>The value.</returns>
    public TValue Get()
    {
        if (HasCachedValue && UsesValueCache)
            return CachedValue;

        ExecuteTriggers(TriggerEvents.BeforeGet);

        TValue value = GetValue();

        ExecuteTriggers(TriggerEvents.AfterGet);

        if (UsesValueCache)
        {
            CachedValue = value;
            HasCachedValue = true;
        }

        return value;
    }

    string? IConvertsValueToString<TValue>.ConvertValueToString(TValue value) =>
        ConvertValueToString(value);

    /// <summary>
    /// Converts the value to string.
    /// Can use format from <see cref="FormatAttribute"/>.
    /// Can use culture from <see cref="CultureAttribute"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The value converted to string.</returns>
    protected internal virtual string? ConvertValueToString(TValue value) =>
        TermResolver.ToString(value, GetValueTermOptions());

    /// <summary>
    /// Converts the string to value of <typeparamref name="TValue"/> type.
    /// Can use format from <see cref="FormatAttribute"/>.
    /// Can use culture from <see cref="CultureAttribute"/>.
    /// </summary>
    /// <param name="value">The value as string.</param>
    /// <returns>The value converted to <typeparamref name="TValue"/> type.</returns>
    protected internal virtual TValue? ConvertStringToValue(string? value) =>
        TermResolver.FromString<TValue>(value, GetValueTermOptions());

    /// <summary>
    /// Converts the string to value of <typeparamref name="TValue"/> type for <see cref="GetValue"/> method.
    /// Can use format from <see cref="ValueGetFormatAttribute"/>,
    /// otherwise from <see cref="FormatAttribute"/>.
    /// Can use culture from <see cref="CultureAttribute"/>.
    /// </summary>
    /// <param name="value">The value as string.</param>
    /// <returns>The value converted to <typeparamref name="TValue"/> type.</returns>
    protected virtual TValue? ConvertStringToValueUsingGetFormat(string? value)
    {
        string? getFormat = Metadata.Get<ValueGetFormatAttribute>()?.Value;

        return getFormat is not null
            ? TermResolver.FromString<TValue>(value, new TermOptions().MergeWith(GetValueTermOptions()).WithFormat(getFormat))
            : ConvertStringToValue(value);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        return obj switch
        {
            Field<TValue, TOwner> objAsField =>
                ReferenceEquals(this, objAsField),
            TValue objAsValue =>
                Equals(objAsValue),
            _ => false
        };
    }

    public bool Equals(TValue other)
    {
        TValue value = Get();
        return Equals(value, other);
    }

    public override int GetHashCode() =>
        ComponentFullName.GetHashCode();

    /// <summary>
    /// Gets the value term options (culture, format, etc.).
    /// </summary>
    /// <returns>The <see cref="TermOptions"/> instance.</returns>
    protected virtual TermOptions GetValueTermOptions() =>
        new()
        {
            Culture = Metadata.GetCulture()
                ?? Session.Context.Culture,
            Format = Metadata.GetFormat()
        };

    protected override void OnClearCache()
    {
        base.OnClearCache();

        ClearValueCache();
    }

    /// <summary>
    /// Clears the column header texts of the component.
    /// </summary>
    /// <returns>The instance of the owner page object.</returns>
    public TOwner ClearValueCache()
    {
        if (HasCachedValue)
        {
            var cachedValue = CachedValue;
            CachedValue = default;
            HasCachedValue = false;
            Log.Trace($"Cleared value cache of {ComponentFullName}: {Stringifier.ToString(cachedValue)}");
        }

        return Owner;
    }
}
