using System;

namespace Atata
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfRawGeneric(this Type type, Type genericType)
        {
            return GetDepthOfInheritanceOfRawGeneric(type, genericType) != null;
        }

        public static int? GetDepthOfInheritanceOfRawGeneric(this Type type, Type genericType)
        {
            Type typeToCheck = type;
            int depth = 0;
            while (typeToCheck != null && typeToCheck != typeof(object))
            {
                if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                    return depth;

                typeToCheck = typeToCheck.BaseType;
                depth++;
            }
            return null;
        }
    }
}
