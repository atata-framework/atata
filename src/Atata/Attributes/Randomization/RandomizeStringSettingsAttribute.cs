namespace Atata;

/// <summary>
/// Specifies the settings for string randomization.
/// </summary>
public class RandomizeStringSettingsAttribute : MulticastAttribute
{
    internal const string DefaultFormat = "{0}";

    internal const int DefaultNumberOfCharacters = 15;

    public RandomizeStringSettingsAttribute(string format = DefaultFormat, int numberOfCharacters = DefaultNumberOfCharacters)
    {
        Format = format;
        NumberOfCharacters = numberOfCharacters;
    }

    /// <summary>
    /// Gets the format.
    /// </summary>
    public string Format { get; }

    /// <summary>
    /// Gets the number of characters to randomize.
    /// </summary>
    public int NumberOfCharacters { get; }
}
