using System;
using System.Text;

namespace Atata
{
    public abstract class Field<T, TOwner> : Control<TOwner>, IEquatable<T>
        where TOwner : PageObject<TOwner>
    {
        protected Field()
        {
        }

        protected TermOptions ValueTermOptions { get; private set; }

        protected abstract T GetValue();

        public TOwner Get(out T value)
        {
            value = GetValue();
            return Owner;
        }

        public T Get()
        {
            return GetValue();
        }

        protected internal virtual string ConvertValueToString(T value)
        {
            return TermResolver.ToString(value, ValueTermOptions);
        }

        protected internal virtual T ConvertStringToValue(string value)
        {
            return TermResolver.FromString<T>(value, ValueTermOptions);
        }

        public TOwner Verify(Action<T> assertAction, string message, params object[] args)
        {
            StringBuilder messageBuilder = new StringBuilder(ComponentName);
            if (!string.IsNullOrWhiteSpace(message))
                messageBuilder.Append(" ").AppendFormat(message, args);

            Log.StartVerificationSection(messageBuilder.ToString());

            T actualValue = GetValue();
            assertAction(actualValue);

            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyEquals(T value)
        {
            return Verify(actual => Assert.AreEqual(value, actual, "Invalid {0} value", ComponentName), "is equal to '{0}'", ConvertValueToString(value));
        }

        public TOwner VerifyDoesNotEqual(T value)
        {
            return Verify(actual => Assert.AreNotEqual(value, actual, "Invalid {0} value", ComponentName), "is not equal to '{0}'", ConvertValueToString(value));
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
                return object.ReferenceEquals(this, objAsField);
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
            return field == null ? object.Equals(value, null) : field.Equals(value);
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
            termOptions.StringFormat = metadata.GetFormat(GetType());
        }
    }
}
