namespace Atata;

public class PropertyBag
{
    private readonly Dictionary<string, object?> _values = [];

    public object? this[string name]
    {
        get => _values[name];
        set => _values[name] = value;
    }

    public bool Contains(string name) =>
        _values.ContainsKey(name);

    public T? GetOrDefault<T>(string name, T? defaultValue = default) =>
        _values.TryGetValue(name, out object? value)
            ? (T?)value
            : defaultValue;

    public bool TryGet<T>(string name, out T? result)
        where T : notnull
    {
        if (_values.TryGetValue(name, out object? value))
        {
            result = (T?)value;
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    public T? Resolve<T>(string name, IEnumerable<IHasOptionalProperties>? optionalPropertiesHolders) =>
        Resolve<T>(name, optionalPropertiesHolders?.Select(x => x.OptionalProperties));

    public T? Resolve<T>(string name, T? defaultValue, IEnumerable<IHasOptionalProperties>? optionalPropertiesHolders) =>
        Resolve(name, defaultValue, optionalPropertiesHolders?.Select(x => x.OptionalProperties));

    public T? Resolve<T>(string name, IEnumerable<PropertyBag>? otherPropertyBags) =>
        Resolve(name, default(T?), otherPropertyBags);

    public T? Resolve<T>(string name, T? defaultValue, IEnumerable<PropertyBag>? otherPropertyBags)
    {
        if (_values.TryGetValue(name, out object? value))
            return (T?)value;

        if (otherPropertyBags is not null)
        {
            foreach (PropertyBag otherPropertyBag in otherPropertyBags)
            {
                if (otherPropertyBag is not null && otherPropertyBag._values.TryGetValue(name, out object? otherValue))
                    return (T?)otherValue;
            }
        }

        return defaultValue;
    }
}
