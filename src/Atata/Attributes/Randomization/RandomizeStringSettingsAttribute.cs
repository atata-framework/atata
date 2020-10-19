namespace Atata
{
    /// <summary>
    /// Specifies the settings for string randomization.
    /// </summary>
    public class RandomizeStringSettingsAttribute : MulticastAttribute
    {
        public RandomizeStringSettingsAttribute(string format = "{0}", int numberOfCharacters = 15)
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
}
