namespace Atata;

/// <summary>
/// Provides a method that converts a value to string.
/// </summary>
/// <typeparam name="TValue">The type of the value.</typeparam>
public interface IConvertsValueToString<in TValue>
{
    /// <summary>
    /// Converts the value to string.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The value converted to string.</returns>
    string? ConvertValueToString(TValue value);
}
