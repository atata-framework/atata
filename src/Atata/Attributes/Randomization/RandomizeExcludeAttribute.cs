using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RandomizeExcludeAttribute : Attribute
    {
        public RandomizeExcludeAttribute(params object[] values)
        {
            Values = values;
        }

        public object[] Values { get; private set; }
    }
}
