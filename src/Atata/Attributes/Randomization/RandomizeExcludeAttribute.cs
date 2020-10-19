namespace Atata
{
    /// <summary>
    /// Specifies the values to exlude during randomization.
    /// </summary>
    public class RandomizeExcludeAttribute : MulticastAttribute
    {
        public RandomizeExcludeAttribute(params object[] values)
        {
            Values = values;
        }

        public object[] Values { get; private set; }
    }
}
