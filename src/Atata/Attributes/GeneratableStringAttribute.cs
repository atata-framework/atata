using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GeneratableStringAttribute : Attribute
    {
        public GeneratableStringAttribute(string format = "{0}", int numberOfCharacters = 15)
        {
            Format = format;
            NumberOfCharacters = numberOfCharacters;
        }

        public string Format { get; private set; }
        public int NumberOfCharacters { get; private set; }
    }
}
