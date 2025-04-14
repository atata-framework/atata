namespace Atata;

public class ObjectCreator : IObjectCreator
{
    private readonly IObjectConverter _objectConverter;

    private readonly IObjectMapper _objectMapper;

    public ObjectCreator(IObjectConverter objectConverter, IObjectMapper objectMapper)
    {
        _objectConverter = objectConverter;
        _objectMapper = objectMapper;
    }

    /// <inheritdoc/>
    public object Create(Type type, Dictionary<string, object?> valuesMap) =>
        Create(type, valuesMap, []);

    /// <inheritdoc/>
    public object Create(Type type, Dictionary<string, object?> valuesMap, Dictionary<string, string> alternativeParameterNamesMap)
    {
        Guard.ThrowIfNull(type);
        Guard.ThrowIfNull(valuesMap);
        Guard.ThrowIfNull(alternativeParameterNamesMap);

        if (valuesMap.Count == 0)
            return ActivatorEx.CreateInstance(type);

        string[] parameterNamesWithAlternatives = [
            .. valuesMap.Keys,
            .. GetAlternativeParameterNames(valuesMap.Keys, alternativeParameterNamesMap)
        ];

        ConstructorInfo constructor = FindMostAppropriateConstructor(type, parameterNamesWithAlternatives);

        Dictionary<string, object?> workingValuesMap = new(valuesMap);

        object instance = CreateInstanceViaConstructorAndRemoveUsedValues(
            constructor,
            workingValuesMap,
            alternativeParameterNamesMap);

        _objectMapper.Map(workingValuesMap, instance);

        return instance;
    }

    private static ConstructorInfo FindMostAppropriateConstructor(Type type, IEnumerable<string> parameterNames) =>
        type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
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

    private static IEnumerable<string> GetAlternativeParameterNames(
        IEnumerable<string> parameterNames,
        Dictionary<string, string> alternativeParameterNamesMap)
    {
        foreach (string parameterName in parameterNames)
        {
            if (TryGetAlternativeParameterName(alternativeParameterNamesMap, parameterName, out string? alternativeParameterName))
                yield return alternativeParameterName;
        }
    }

    private static bool TryGetAlternativeParameterName(
        Dictionary<string, string> alternativeParameterNamesMap,
        string parameterName,
        [NotNullWhen(true)] out string? alternativeParameterName)
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

    private object CreateInstanceViaConstructorAndRemoveUsedValues(
        ConstructorInfo constructor,
        Dictionary<string, object?> valuesMap,
        Dictionary<string, string> alternativeParameterNamesMap)
    {
        object?[] arguments = constructor.GetParameters()
            .Select(parameter =>
            {
                KeyValuePair<string, object?> valuePair = RetrievePairByName(valuesMap, alternativeParameterNamesMap, parameter);

                valuesMap.Remove(valuePair.Key);

                return _objectConverter.Convert(valuePair.Value, parameter.ParameterType);
            })
            .ToArray();

        return constructor.Invoke(arguments);
    }

    private static KeyValuePair<string, object?> RetrievePairByName(
        Dictionary<string, object?> valuesMap,
        Dictionary<string, string> alternativeParameterNamesMap,
        ParameterInfo parameter)
    {
        KeyValuePair<string, object?> valuePair = valuesMap.FirstOrDefault(pair =>
        {
            if (pair.Key.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase))
                return true;
            else if (TryGetAlternativeParameterName(alternativeParameterNamesMap, pair.Key, out string? alternativeParameterName))
                return alternativeParameterName.Equals(parameter.Name, StringComparison.OrdinalIgnoreCase);
            else
                return false;
        });

        if (valuePair.Key != null)
            return valuePair;
        else if (parameter.IsOptional)
            return new(parameter.Name, parameter.DefaultValue);
        else if (parameter.GetCustomAttributes(true).Any(attr => attr is ParamArrayAttribute))
            return new(parameter.Name, null);
        else
            throw new InvalidOperationException($"Failed to find \"{parameter.Name}\" required constructor parameter value.");
    }
}
