using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public static class TypeExtensions
    {
        internal static bool IsClassOrNullable(this Type type)
        {
            type.CheckNotNull(nameof(type));

            return type.IsClass || Nullable.GetUnderlyingType(type) != null;
        }

        public static Enum[] GetIndividualEnumFlags(this Type type)
        {
            type.CheckNotNull(nameof(type));

            if (type.IsDefined(typeof(FlagsAttribute), false))
            {
                Enum[] allFlags = Enum.GetValues(type).Cast<Enum>().Where(x => Convert.ToDecimal(x) != 0m).ToArray();
                List<Enum> individualFlags = new List<Enum>(allFlags.Take(1));

                for (int i = 1; i < allFlags.Length; i++)
                {
                    Enum currentFlag = allFlags[i];
                    if (allFlags.Take(i).All(x => !currentFlag.HasFlag(x)))
                        individualFlags.Add(currentFlag);
                }

                return individualFlags.ToArray();
            }
            else
            {
                return Enum.GetValues(type).Cast<Enum>().ToArray();
            }
        }

        internal static MethodInfo GetMethodWithThrowOnError(this Type type, string name, params Type[] types)
        {
            type.CheckNotNull(nameof(type));
            name.CheckNotNullOrWhitespace(nameof(name));

            MethodInfo method = type.GetMethod(name, types);

            if (method == null)
                throw new MissingMethodException(type.FullName, name);

            return method;
        }

        internal static MethodInfo GetMethodWithThrowOnError(this Type type, string name, BindingFlags bindingFlags)
        {
            type.CheckNotNull(nameof(type));
            name.CheckNotNullOrWhitespace(nameof(name));

            MethodInfo method = type.GetMethod(name, bindingFlags);

            if (method == null)
                throw new MissingMethodException(type.FullName, name);

            return method;
        }

        internal static MethodInfo GetMethodWithThrowOnError(this Type type, string name, BindingFlags bindingFlags, params Type[] types)
        {
            type.CheckNotNull(nameof(type));
            name.CheckNotNullOrWhitespace(nameof(name));

            MethodInfo method = type.GetMethod(name, bindingFlags, null, types, null);

            if (method == null)
                throw new MissingMethodException(type.FullName, name);

            return method;
        }

        internal static PropertyInfo GetPropertyWithThrowOnError(this Type type, string name, BindingFlags bindingFlags = BindingFlags.Default)
        {
            type.CheckNotNull(nameof(type));
            name.CheckNotNullOrWhitespace(nameof(name));

            PropertyInfo[] properties = bindingFlags == BindingFlags.Default
                ? type.GetProperties()
                : type.GetProperties(bindingFlags);

            PropertyInfo property = properties.FirstOrDefault(x => x.Name == name);

            if (property == null)
                throw new MissingMemberException(type.FullName, name);

            return property;
        }

        internal static PropertyInfo GetPropertyWithThrowOnError(this Type type, string name, Type propertyType, BindingFlags bindingFlags = BindingFlags.Default)
        {
            type.CheckNotNull(nameof(type));
            name.CheckNotNullOrWhitespace(nameof(name));

            if (propertyType == null)
                return type.GetPropertyWithThrowOnError(name, bindingFlags);

            PropertyInfo[] properties = bindingFlags == BindingFlags.Default
                ? type.GetProperties()
                : type.GetProperties(bindingFlags);

            PropertyInfo property = properties.FirstOrDefault(x => x.Name == name && x.PropertyType.IsAssignableFrom(propertyType));

            if (property == null)
                throw new MissingMemberException(type.FullName, name);

            return property;
        }

        internal static FieldInfo GetFieldWithThrowOnError(this Type type, string name, BindingFlags bindingFlags = BindingFlags.Default)
        {
            type.CheckNotNull(nameof(type));
            name.CheckNotNullOrWhitespace(nameof(name));

            FieldInfo[] fields = bindingFlags == BindingFlags.Default
                ? type.GetFields()
                : type.GetFields(bindingFlags);

            FieldInfo field = fields.FirstOrDefault(x => x.Name == name);

            if (field == null)
                throw new MissingMemberException(type.FullName, name);

            return field;
        }

        public static int? GetDepthOfInheritance(this Type type, Type baseType)
        {
            type.CheckNotNull(nameof(type));

            if (baseType == null)
                return null;
            else if (baseType.IsGenericTypeDefinition)
                return type.GetDepthOfInheritanceOfRawGeneric(baseType);
            else
                return GetDepthOfInheritanceOfRegularType(type, baseType);
        }

        private static int? GetDepthOfInheritanceOfRegularType(Type type, Type baseType)
        {
            Type typeToCheck = type;

            for (int depth = 0; typeToCheck != null; depth++)
            {
                if (typeToCheck == baseType)
                    return depth;

                typeToCheck = typeToCheck.BaseType;
            }

            return null;
        }

        public static bool IsInheritedFromOrIs(this Type type, Type baseType)
        {
            type.CheckNotNull(nameof(type));

            if (baseType == null)
                return false;
            else if (baseType.IsGenericTypeDefinition)
                return type.GetDepthOfInheritanceOfRawGeneric(baseType) != null;
            else
                return baseType.IsAssignableFrom(type);
        }
    }
}
