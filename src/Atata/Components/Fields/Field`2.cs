using System;

namespace Atata
{
    public abstract class Field<T, TOwner> : Control<TOwner>, IEquatable<T>
        where TOwner : PageObject<TOwner>
    {
        protected Field()
        {
        }

        protected abstract T GetValue();

        public TOwner Get(out T value)
        {
            value = GetValue();
            return Owner;
        }

        public TOwner VerifyEquals(T value)
        {
            Log.StartVerificationSection("{0} equals '{1}'", ComponentName, value);

            T actualValue = GetValue();
            Asserter.AreEqual(value, actualValue, "Invalid {0} value", ComponentName);

            Log.EndSection();
            return Owner;
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
    }
}
