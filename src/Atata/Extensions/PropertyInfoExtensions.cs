using System.Reflection;

namespace Atata
{
    internal static class PropertyInfoExtensions
    {
        internal static object GetValue(this PropertyInfo property, object obj)
        {
            return property.GetValue(obj, new object[0]);
        }

        internal static object GetStaticValue(this PropertyInfo property)
        {
            return property.GetValue(null, new object[0]);
        }
    }
}
