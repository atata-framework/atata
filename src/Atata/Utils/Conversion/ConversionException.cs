﻿namespace Atata;

public class ConversionException : Exception
{
    public ConversionException()
    {
    }

    public ConversionException(string? message)
        : base(message)
    {
    }

    public ConversionException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public static ConversionException For(object? sourceValue, Type destinationType) =>
        new(
            sourceValue is null
                ? $"Cannot convert null value to {destinationType.FullName} type."
                : $"Cannot convert \"{sourceValue}\" value of {sourceValue.GetType().FullName} type to {destinationType.FullName} type.");
}
