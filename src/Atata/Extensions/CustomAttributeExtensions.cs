using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class CustomAttributeExtensions
    {
        internal static T GetCustomAttribute<T>(this MemberInfo element, bool inherit = true)
            where T : Attribute
        {
            return element.GetCustomAttributes(typeof(T), inherit).Cast<T>().FirstOrDefault();
        }

        public static bool TryGetCustomAttribute<T>(this MemberInfo element, out T attribute, bool inherit = true)
            where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>(inherit);
            return attribute != null;
        }
    }
}
