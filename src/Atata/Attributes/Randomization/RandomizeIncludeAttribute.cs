namespace Atata
{
    /// <summary>
    /// Specifies the values to choose during randomization.
    /// </summary>
    public class RandomizeIncludeAttribute : MulticastAttribute
    {
        public RandomizeIncludeAttribute(params object[] values) =>
            Values = values;

        public object[] Values { get; }
    }
}
