using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RandomizeNumberSettingsAttribute : Attribute
    {
        public RandomizeNumberSettingsAttribute(int min = 0, int max = 100)
            : this((decimal)min, (decimal)max, 0)
        {
        }

        public RandomizeNumberSettingsAttribute(double min, double max, int precision = 0)
            : this((decimal)min, (decimal)max, precision)
        {
        }

        private RandomizeNumberSettingsAttribute(decimal min, decimal max, int precision)
        {
            Min = min;
            Max = max;
            Precision = precision;
        }

        public decimal Min { get; private set; }
        public decimal Max { get; private set; }
        public int Precision { get; private set; }
    }
}
