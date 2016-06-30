using System;

namespace Atata
{
    /// <summary>
    /// Represents the base class for the field controls.
    /// </summary>
    /// <typeparam name="T">The type of the control's data.</typeparam>
    /// <typeparam name="TOwner">The type of the owner page object.</typeparam>
    [ControlFinding(FindTermBy.Label)]
    public abstract class Field<T, TOwner> : Control<TOwner>, IEquatable<T>, IUIComponentValueProvider<T, TOwner>
        where TOwner : PageObject<TOwner>
    {
        protected Field()
        {
        }

        protected TermOptions ValueTermOptions { get; private set; }

        string IUIComponentValueProvider<T, TOwner>.ComponentFullName
        {
            get { return ComponentFullName; }
        }

        protected virtual string ValueProviderName
        {
            get { return "value"; }
        }

        string IUIComponentValueProvider<T, TOwner>.ProviderName
        {
            get { return ValueProviderName; }
        }

        TOwner IUIComponentValueProvider<T, TOwner>.Owner
        {
            get { return Owner; }
        }

        protected abstract T GetValue();

        public T Get()
        {
            return GetValue();
        }

        string IUIComponentValueProvider<T, TOwner>.ConvertValueToString(T value)
        {
            return ConvertValueToString(value);
        }

        protected internal virtual string ConvertValueToString(T value)
        {
            return TermResolver.ToString(value, ValueTermOptions);
        }

        protected internal virtual T ConvertStringToValue(string value)
        {
            return TermResolver.FromString<T>(value, ValueTermOptions);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            Field<T, TOwner> objAsField = obj as Field<T, TOwner>;
            if (objAsField != null)
            {
                return ReferenceEquals(this, objAsField);
            }
            else if (obj is T)
            {
                T objAsValue = (T)obj;
                return Equals(objAsValue);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(T other)
        {
            T value = GetValue();
            return object.Equals(value, other);
        }

        public static bool operator ==(Field<T, TOwner> field, T value)
        {
            return field == null ? Equals(value, null) : field.Equals(value);
        }

        public static bool operator !=(Field<T, TOwner> field, T value)
        {
            return !(field == value);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            ValueTermOptions = TermOptions.CreateDefault();
            InitValueTermOptions(ValueTermOptions, metadata);
        }

        protected virtual void InitValueTermOptions(TermOptions termOptions, UIComponentMetadata metadata)
        {
            termOptions.Culture = metadata.GetCulture();
            termOptions.Format = metadata.GetFormat();
        }
    }
}
