using System;

namespace Atata
{
    /// <summary>
    /// Represents the base class for the field controls.
    /// It can be used for HTML elements containing content
    /// (like <c>&lt;h1&gt;</c>, <c>&lt;span&gt;</c>, etc.) representing content as a field value,
    /// as well as for <c>&lt;input&gt;</c> and other elements.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    public abstract class Field<T, TOwner> : Control<TOwner>, IEquatable<T>, IObjectProvider<T, TOwner>, IConvertsValueToString<T>
        where TOwner : PageObject<TOwner>
    {
        protected Field()
        {
        }

        protected T CachedValue { get; private set; }

        protected bool HasCachedValue { get; private set; }

        protected bool UsesValueCache =>
            Metadata.Get<ICanUseCache>(filter => filter.Where(x => x is UsesCacheAttribute || x is UsesValueCacheAttribute))
                ?.UsesCache ?? false;

        /// <summary>
        /// Gets the value.
        /// Also executes <see cref="TriggerEvents.BeforeGet"/> and <see cref="TriggerEvents.AfterGet"/> triggers.
        /// </summary>
        public T Value => Get();

        /// <summary>
        /// Gets the name of the value provider.
        /// The default value is <c>"value"</c>.
        /// </summary>
        protected virtual string ValueProviderName => "value";

        string IObjectProvider<T>.ProviderName =>
            $"{BuildComponentProviderName()} {ValueProviderName}";

        TOwner IDataProvider<T, TOwner>.Owner => Owner;

        bool IObjectProvider<T, TOwner>.IsValueDynamic => true;

        /// <summary>
        /// Gets the assertion verification provider that has a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<T, Field<T, TOwner>, TOwner> Should => new FieldVerificationProvider<T, Field<T, TOwner>, TOwner>(this);

        /// <summary>
        /// Gets the expectation verification provider that has a set of verification extension methods.
        /// </summary>
        public new FieldVerificationProvider<T, Field<T, TOwner>, TOwner> ExpectTo => Should.Using<ExpectationVerificationStrategy>();

        /// <summary>
        /// Gets the waiting verification provider that has a set of verification extension methods.
        /// Uses <see cref="AtataContext.WaitingTimeout"/> and <see cref="AtataContext.WaitingRetryInterval"/> of <see cref="AtataContext.Current"/> for timeout and retry interval.
        /// </summary>
        public new FieldVerificationProvider<T, Field<T, TOwner>, TOwner> WaitTo => Should.Using<WaitingVerificationStrategy>();

        public static explicit operator T(Field<T, TOwner> field)
        {
            return field.Get();
        }

        public static bool operator ==(Field<T, TOwner> field, T value)
        {
            return field == null ? Equals(value, null) : field.Equals(value);
        }

        public static bool operator !=(Field<T, TOwner> field, T value)
        {
            return !(field == value);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value.</returns>
        protected abstract T GetValue();

        /// <summary>
        /// Gets the value. Also executes <see cref="TriggerEvents.BeforeGet"/> and <see cref="TriggerEvents.AfterGet"/> triggers.
        /// </summary>
        /// <returns>The value.</returns>
        public T Get()
        {
            if (HasCachedValue && UsesValueCache)
                return CachedValue;

            ExecuteTriggers(TriggerEvents.BeforeGet);

            T value = GetValue();

            ExecuteTriggers(TriggerEvents.AfterGet);

            if (UsesValueCache)
            {
                CachedValue = value;
                HasCachedValue = true;
            }

            return value;
        }

        string IConvertsValueToString<T>.ConvertValueToString(T value) =>
            ConvertValueToString(value);

        /// <summary>
        /// Converts the value to string.
        /// Can use format from <see cref="FormatAttribute"/>.
        /// Can use culture from <see cref="CultureAttribute"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The value converted to string.</returns>
        protected internal virtual string ConvertValueToString(T value) =>
            TermResolver.ToString(value, GetValueTermOptions());

        /// <summary>
        /// Converts the string to value of <typeparamref name="T"/> type.
        /// Can use format from <see cref="FormatAttribute"/>.
        /// Can use culture from <see cref="CultureAttribute"/>.
        /// </summary>
        /// <param name="value">The value as string.</param>
        /// <returns>The value converted to <typeparamref name="T"/> type.</returns>
        protected internal virtual T ConvertStringToValue(string value)
        {
            return TermResolver.FromString<T>(value, GetValueTermOptions());
        }

        /// <summary>
        /// Converts the string to value of <typeparamref name="T"/> type for <see cref="GetValue"/> method.
        /// Can use format from <see cref="ValueGetFormatAttribute"/>,
        /// otherwise from <see cref="FormatAttribute"/>.
        /// Can use culture from <see cref="CultureAttribute"/>.
        /// </summary>
        /// <param name="value">The value as string.</param>
        /// <returns>The value converted to <typeparamref name="T"/> type.</returns>
        protected virtual T ConvertStringToValueUsingGetFormat(string value)
        {
            string getFormat = Metadata.Get<ValueGetFormatAttribute>()?.Value;

            return getFormat != null
                ? TermResolver.FromString<T>(value, new TermOptions().MergeWith(GetValueTermOptions()).WithFormat(getFormat))
                : ConvertStringToValue(value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            switch (obj)
            {
                case Field<T, TOwner> objAsField:
                    return ReferenceEquals(this, objAsField);
                case T objAsValue:
                    return Equals(objAsValue);
                default:
                    return false;
            }
        }

        public bool Equals(T other)
        {
            T value = Get();
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
