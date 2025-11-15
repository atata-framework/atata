namespace Atata;

internal sealed class TypeExpressionValueStringifier : IExpressionValueStringifier
{
    bool IExpressionValueStringifier.CanHandle(Type type) =>
        type == typeof(Type);

    string IExpressionValueStringifier.Handle(object value) =>
        $"typeof({((Type)value).ToStringInShortForm()})";
}
