using System;
using System.Linq;

namespace Atata
{
    public static class TypeExtensions
    {
        public static bool IsSubclassOfRawGeneric(this Type type, Type genericType)
        {
            return type.GetDepthOfInheritanceOfRawGeneric(genericType) != null;
        }

        public static int? GetDepthOfInheritanceOfRawGeneric(this Type type, Type genericType)
        {
            if (genericType == null)
                return null;

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

        public static bool IsImplementGenericInterface(this Type type, Type genericType)
        {
            return type.GetGenericInterfaceType(genericType) != null;
        }

        public static Type GetGenericInterfaceType(this Type type, Type genericType)
        {
            return type.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericType);
        }
    }
}
