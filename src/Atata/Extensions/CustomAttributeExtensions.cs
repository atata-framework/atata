using System;
using System.Reflection;

namespace Atata
{
    public static class CustomAttributeExtensions
    {
        public static bool TryGetCustomAttribute<T>(this MemberInfo element, out T attribute, bool inherit = false)
            where T : Attribute
        {
            attribute = element.GetCustomAttribute<T>(inherit);
            return attribute != null;
        }
    }
}
