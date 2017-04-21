using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RandomizeCountAttribute : Attribute
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

        public int Min { get; private set; }

        public int Max { get; private set; }
    }
}
