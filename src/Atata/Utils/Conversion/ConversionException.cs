using System;
using System.Runtime.Serialization;

namespace Atata
{
    [Serializable]
    public class ConversionException : Exception
    {
        public ConversionException()
        {
        }

        public ConversionException(string message)
            : base(message)
        {
        }

        public ConversionException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ConversionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public static ConversionException For(object sourceValue, Type destinationType)
        {
            return new ConversionException(
                sourceValue == null
                ? $"Cannot convert null value to {destinationType.FullName} type."
                : $"Cannot convert \"{sourceValue}\" value of {sourceValue.GetType().FullName} type to {destinationType.FullName} type.");
        }
    }
}
