using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RandomizeIncludeAttribute : Attribute
    {
        public RandomizeIncludeAttribute(params object[] values)
        {
            Values = values;
        }

        public object[] Values { get; }
    }
}
