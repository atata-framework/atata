using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GeneratableStringAttribute : Attribute
    {
        public GeneratableStringAttribute(string prefix = null, int numberOfCharacters = 15, string separator = " ")
        {
            Prefix = prefix;
            NumberOfCharacters = numberOfCharacters;
            Separator = separator;
        }

        public string Prefix { get; private set; }
        public int NumberOfCharacters { get; private set; }
        public string Separator { get; private set; }
    }
}
