using System;

namespace Atata
{
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
        protected Field()
        {
        }

        protected TValue CachedValue { get; private set; }

        protected bool HasCachedValue { get; private set; }

        protected bool UsesValueCache =>
            Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute || x is UsesValueCacheAttribute))
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
            $"{BuildComponentProviderName()} {ValueProviderName}";

        TOwner IObjectProvider<TValue, TOwner>.Owner => Owner;

        bool IObjectProvider<TValue, TOwner>.IsDynamic => true;

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<TValue, Field<TValue, TOwner>, TOwner> Should => new FieldVerificationProvider<TValue, Field<TValue, TOwner>, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<TValue, Field<TValue, TOwner>, TOwner> ExpectTo => Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public new FieldVerificationProvider<TValue, Field<TValue, TOwner>, TOwner> WaitTo => Should.Using<WaitingVerificationStrategy>();

        public static explicit operator TValue(Field<TValue, TOwner> field) =>
            field.Get();

        public static bool operator ==(Field<TValue, TOwner> field, TValue value) =>
            field == null
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

        string IConvertsValueToString<TValue>.ConvertValueToString(TValue value) =>
            ConvertValueToString(value);

        /// <summary>
        /// Converts the value to string.
        /// Can use format from <see cref="FormatAttribute"/>.
        /// Can use culture from <see cref="CultureAttribute"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value converted to string.</returns>
        protected internal virtual string ConvertValueToString(TValue value) =>
            TermResolver.ToString(value, GetValueTermOptions());

        /// <summary>
        /// Converts the string to value of <typeparamref name="TValue"/> type.
        /// Can use format from <see cref="FormatAttribute"/>.
        /// Can use culture from <see cref="CultureAttribute"/>.
        /// </summary>
        /// <param name="value">The value as string.</param>
        /// <returns>The value converted to <typeparamref name="TValue"/> type.</returns>
        protected internal virtual TValue ConvertStringToValue(string value)
        {
            return TermResolver.FromString<TValue>(value, GetValueTermOptions());
        }

        /// <summary>
        /// Converts the string to value of <typeparamref name="TValue"/> type for <see cref="GetValue"/> method.
        /// Can use format from <see cref="ValueGetFormatAttribute"/>,
        /// otherwise from <see cref="FormatAttribute"/>.
        /// Can use culture from <see cref="CultureAttribute"/>.
        /// </summary>
        /// <param name="value">The value as string.</param>
        /// <returns>The value converted to <typeparamref name="TValue"/> type.</returns>
        protected virtual TValue ConvertStringToValueUsingGetFormat(string value)
        {
            string getFormat = Metadata.Get<ValueGetFormatAttribute>()?.Value;

            return getFormat != null
                ? TermResolver.FromString<TValue>(value, new TermOptions().MergeWith(GetValueTermOptions()).WithFormat(getFormat))
                : ConvertStringToValue(value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            switch (obj)
            {
                case Field<TValue, TOwner> objAsField:
                    return ReferenceEquals(this, objAsField);
                case TValue objAsValue:
                    return Equals(objAsValue);
                default:
                    return false;
            }
        }

        public bool Equals(TValue other)
        {
            TValue value = Get();
            return Equals(value, other);
        }

        public override int GetHashCode()
        {
            return ComponentFullName.GetHashCode();
        }

        /// <summary>
        /// Gets the value term options (culture, format, etc.).
        /// </summary>
        /// <returns>The <see cref="TermOptions"/> instance.</returns>
        protected virtual TermOptions GetValueTermOptions() =>
            new TermOptions
            {
                Culture = Metadata.GetCulture(),
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
}
