namespace Atata;

/// <summary>
/// Specifies the settings for number randomization.
/// </summary>
public class RandomizeNumberSettingsAttribute : MulticastAttribute
{
    internal const decimal DefaultMin = 0m;

    internal const decimal DefaultMax = 100m;

    internal const int DefaultPrecision = 0;

    public RandomizeNumberSettingsAttribute(int min = 0, int max = 100)
        : this((decimal)min, max, 0)
    {
    }

    public RandomizeNumberSettingsAttribute(double min, double max, int precision = DefaultPrecision)
        : this((decimal)min, (decimal)max, precision)
    {
    }

    private RandomizeNumberSettingsAttribute(decimal min, decimal max, int precision)
    {
        Min = min;
        Max = max;
        Precision = precision;
    }

    /// <summary>
    /// Gets the minimum boundary value.
    /// </summary>
    public decimal Min { get; }

    /// <summary>
    /// Gets the maximum boundary value.
    /// </summary>
    public decimal Max { get; }

    /// <summary>
    /// Gets the precision.
    /// </summary>
    public int Precision { get; }
}
