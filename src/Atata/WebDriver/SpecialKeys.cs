namespace Atata;

public static class SpecialKeys
{
    static SpecialKeys() =>
        ValueNameMap = ResolveValueNameMap();

    public static Dictionary<char, string> ValueNameMap { get; }

    private static Dictionary<char, string> ResolveValueNameMap()
    {
        try
        {
            FieldInfo[] fields = typeof(Keys).GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields
                .Select(x => new NameValuePair(x.Name, ((string)x.GetValue(null))[0]))
                .Distinct(new NameValuePairComparer())
                .ToDictionary(x => x.Value, x => x.Name);
        }
        catch
        {
            // For the case when something will change in OpenQA.Selenium.Keys class.
            return [];
        }
    }

    public static string Replace(string keys)
    {
        Guard.ThrowIfNull(keys);

        StringBuilder? builder = null;

        for (int i = 0; i < keys.Length; i++)
        {
            if (ValueNameMap.TryGetValue(keys[i], out string? specialKeyName))
            {
                builder ??= new(keys, 0, i, keys.Length + 12);

                builder.Append('<').Append(specialKeyName).Append('>');
            }
            else
            {
                builder?.Append(keys[i]);
            }
        }

        return builder is null
            ? keys
            : builder.ToString();
    }

    private sealed class NameValuePair
    {
        public NameValuePair(string name, char value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }

        public char Value { get; }
    }

    private sealed class NameValuePairComparer : IEqualityComparer<NameValuePair>
    {
        public bool Equals(NameValuePair? x, NameValuePair? y)
        {
            if (x is null && y is null)
                return true;

            if (x is null || y is null)
                return false;

            return Equals(x.Value, y.Value);
        }

        public int GetHashCode(NameValuePair obj) =>
            obj.Value.GetHashCode();
    }
}
