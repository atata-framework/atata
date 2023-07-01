namespace Atata;

/// <summary>
/// Specifies the count of items to randomize.
/// </summary>
/// <remarks>
/// It can be useful for <see cref="CheckBoxList{TValue, TOwner}"/> to set the number of checkboxes to check.
/// </remarks>
public class RandomizeCountAttribute : MulticastAttribute
{
    public RandomizeCountAttribute(int count)
        : this(count, count)
    {
    }

    public RandomizeCountAttribute(int min, int max)
    {
        Min = min;
        Max = max;
    }

    /// <summary>
    /// Gets the minimum count.
    /// </summary>
    public int Min { get; }

    /// <summary>
    /// Gets the maximum count.
    /// </summary>
    public int Max { get; }
}
