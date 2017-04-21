using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RandomizeStringSettingsAttribute : Attribute
    {
        public RandomizeStringSettingsAttribute(string format = "{0}", int numberOfCharacters = 15)
        {
            Format = format;
            NumberOfCharacters = numberOfCharacters;
        }

        public string Format { get; private set; }

        public int NumberOfCharacters { get; private set; }
    }
}
