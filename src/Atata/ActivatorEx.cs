using System;
using System.Linq;
using System.Reflection;

namespace Atata
{
    internal static class ActivatorEx
    {
        internal static T CreateInstance<T>(string typeName)
        {
            typeName.CheckNotNullOrEmpty(nameof(typeName));

            Type type = Type.GetType(typeName, true);

            return CreateInstance<T>(type);
        }

        internal static T CreateInstance<T>(Type type = null)
        {
            return (T)CreateInstance(type ?? typeof(T));
        }

        internal static object CreateInstance(Type type)
        {
            var constructorData = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).
                Select(x => new { Constructor = x, Parameters = x.GetParameters() }).
                OrderByDescending(x => x.Constructor.IsPublic).
                ThenBy(x => x.Parameters.Length).
                FirstOrDefault(x => !x.Parameters.Any() || x.Parameters.All(param => param.IsOptional || param.GetCustomAttributes(true).Any(attr => attr is ParamArrayAttribute)));

            if (constructorData == null)
                throw new MissingMethodException($"No parameterless constructor or constructor without non-optional parameters defined for the {type.FullName} type.");

            object[] parameters = constructorData.Parameters.Select(x => x.IsOptional ? x.DefaultValue : null).ToArray();

            return constructorData.Constructor.Invoke(parameters);
        }
    }
}
