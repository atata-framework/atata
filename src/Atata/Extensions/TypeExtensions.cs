using System;

namespace Atata
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfRawGeneric(this Type type, Type genericType)
        {
            Type typeToCheck = type;
            while (typeToCheck != null && typeToCheck != typeof(object))
            {
                if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                    return true;

                typeToCheck = typeToCheck.BaseType;
            }
            return false;
        }
    }
}
