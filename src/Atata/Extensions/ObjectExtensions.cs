using System;

namespace Atata
{
    public static class ObjectExtensions
    {
        public static string ToString(this object value, TermFormat format)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return TermResolver.ToString(value, new TermOptions { Format = format });
        }
    }
}
