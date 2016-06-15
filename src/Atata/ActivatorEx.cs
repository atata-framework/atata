using System;
using System.Linq;

namespace Atata
{
    internal static class ActivatorEx
    {
        internal static T CreateInstance<T>(Type type = null)
        {
            return (T)CreateInstance(type ?? typeof(T));
        }

        internal static object CreateInstance(Type type)
        {
            var constructorData = type.GetConstructors().
                Select(x => new { Constructor = x, Parameters = x.GetParameters() }).
                FirstOrDefault(x =>
                {
                    return !x.Parameters.Any() || x.Parameters.All(param => param.IsOptional || param.GetCustomAttributes(true).Any(attr => attr is ParamArrayAttribute));
                });

            if (constructorData == null)
                throw new MissingMethodException("No parameterless constructor or constructor without non-optional parameters defined for the {0} type.".FormatWith(type.FullName));

            object[] parameters = constructorData.Parameters.Select(x => x.IsOptional ? x.DefaultValue : null).ToArray();

            return constructorData.Constructor.Invoke(parameters);
        }
    }
}
