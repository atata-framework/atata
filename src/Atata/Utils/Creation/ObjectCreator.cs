using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atata
{
    public class ObjectCreator : IObjectCreator
    {
        private readonly IObjectConverter objectConverter;

        private readonly IObjectMapper objectMapper;

        private readonly Dictionary<string, string> alternativeParameterNamesMap = new Dictionary<string, string>
        {
            ["value"] = "values"
        };

        public ObjectCreator(IObjectConverter objectConverter, IObjectMapper objectMapper)
        {
            this.objectConverter = objectConverter;
            this.objectMapper = objectMapper;
        }

        /// <inheritdoc/>
        public object Create(Type type, Dictionary<string, object> valuesMap)
        {
            type.CheckNotNull(nameof(type));
            valuesMap.CheckNotNull(nameof(valuesMap));

            if (!valuesMap.Any())
                return ActivatorEx.CreateInstance(type);

            string[] parameterNamesWithAlternatives = valuesMap.Keys
                .Concat(GetAlternativeParameterNames(valuesMap.Keys))
                .ToArray();

            ConstructorInfo constructor = FindMostAppropriateConstructor(type, parameterNamesWithAlternatives);

            var workingValuesMap = new Dictionary<string, object>(valuesMap);

            object instance = CreateInstanceViaConstructorAndRemoveUsedValues(constructor, workingValuesMap);

            objectMapper.Map(workingValuesMap, instance);

            return instance;
        }

        private static ConstructorInfo FindMostAppropriateConstructor(Type type, IEnumerable<string> parameterNames)
        {
            return type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(constructor =>
                {
                    var parameters = constructor.GetParameters();

                    return parameters.Length == 0
                        || parameters.All(parameter =>
                            parameterNames.Contains(parameter.Name, StringComparer.OrdinalIgnoreCase)
                            || parameter.IsOptional
                            || parameter.GetCustomAttributes(true).Any(attr => attr is ParamArrayAttribute));
                })
                .OrderByDescending(x => x.GetParameters().Length)
                .FirstOrDefault()
                ?? throw new MissingMethodException(
                    $"No appropriate constructor found for {type.FullName} type.");
        }

        private IEnumerable<string> GetAlternativeParameterNames(IEnumerable<string> parameterNames)
        {
            foreach (string parameterName in parameterNames)
            {
                if (TryGetAlternativeParameterName(parameterName, out string alternativeParameterName))
                    yield return alternativeParameterName;
            }
        }

        private bool TryGetAlternativeParameterName(string parameterName, out string alternativeParameterName)
        {
            KeyValuePair<string, string> alternativePair = alternativeParameterNamesMap.FirstOrDefault(x => x.Key.Equals(parameterName, StringComparison.OrdinalIgnoreCase));

            if (alternativePair.Key != null)
            {
                alternativeParameterName = alternativePair.Value;
                return true;
            }
            else
            {
                alternativeParameterName = null;
                return false;
            }
        }

        private object CreateInstanceViaConstructorAndRemoveUsedValues(ConstructorInfo constructor, Dictionary<string, object> valuesMap)
        {
            object[] arguments = constructor.GetParameters()
                .Select(parameter =>
                {
                    KeyValuePair<string, object> valuePair = RetrievePairByName(valuesMap, parameter);

                    valuesMap.Remove(valuePair.Key);

                    return objectConverter.Convert(valuePair.Value, parameter.ParameterType);
                })
                .ToArray();

            return constructor.Invoke(arguments);
        }

        private KeyValuePair<string, object> RetrievePairByName(Dictionary<string, object> valuesMap, ParameterInfo parameter)
        {
            KeyValuePair<string, object> valuePair = valuesMap.FirstOrDefault(pair =>
            {
                if (pair.Key.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase))
                    return true;
                else if (TryGetAlternativeParameterName(pair.Key, out string alternativeParameterName))
                    return alternativeParameterName.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase);
                else
                    return false;
            });

            if (valuePair.Key != null)
                return valuePair;
            else if (parameter.IsOptional)
                return new KeyValuePair<string, object>(parameter.Name, parameter.DefaultValue);
            else if (parameter.GetCustomAttributes(true).Any(attr => attr is ParamArrayAttribute))
                return new KeyValuePair<string, object>(parameter.Name, null);
            else
                throw new InvalidOperationException($"Failed to find \"{parameter.Name}\" required constructor parameter value.");
        }
    }
}
