using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GeneratableNumberAttribute : Attribute
    {
        private GeneratableNumberAttribute(decimal min, decimal max, int precision)
        {
            Min = min;
            Max = max;
            Precision = precision;
        }

        public GeneratableNumberAttribute(int min, int max, int precision = 0)
            : this((decimal)min, (decimal)max, precision)
        {
        }

        public GeneratableNumberAttribute(double min, double max, int precision = 0)
            : this((decimal)min, (decimal)max, precision)
        {
        }

        public decimal Min { get; private set; }
        public decimal Max { get; private set; }
        public int Precision { get; private set; }
    }
}
