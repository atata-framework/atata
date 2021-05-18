using System.Reflection;

namespace Atata
{
    internal static class PropertyInfoExtensions
    {
        internal static object GetValue(this PropertyInfo property, object obj) =>
            property.GetValue(obj, null);

        internal static object GetStaticValue(this PropertyInfo property) =>
            property.GetValue(null, null);

        internal static void SetStaticValue(this PropertyInfo property, object value) =>
            property.SetValue(null, value, null);
    }
}
