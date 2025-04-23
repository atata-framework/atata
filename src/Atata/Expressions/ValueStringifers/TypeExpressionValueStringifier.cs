namespace Atata;

internal sealed class TypeExpressionValueStringifier : IExpressionValueStringifier
{
    public bool CanHandle(Type type) =>
        type == typeof(Type);

    public string Handle(object value) =>
        $"typeof({((Type)value).ToStringInShortForm()})";
}
