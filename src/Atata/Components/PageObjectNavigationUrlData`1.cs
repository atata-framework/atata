#nullable enable

namespace Atata;

internal sealed class PageObjectNavigationUrlData<TPageObject>
    where TPageObject : PageObject<TPageObject>
{
    internal bool Appends { get; set; }

    internal string? Value { get; set; }

    internal Dictionary<string, object?>? Variables { get; private set; }

    internal void Set(string? url)
    {
        Value = url;
        Appends = false;
    }

    internal void Append(string urlPart)
    {
        if (urlPart?.Length > 0)
        {
            Value = Value is null
                ? urlPart
                : UriUtils.MergeAsString(Value, urlPart);
        }

        Appends = true;
    }

    internal void SetVariable(string key, object? value)
    {
        key.CheckNotNullOrWhitespace(nameof(key));

        Variables ??= [];
        Variables[key] = value;
    }
}
