namespace Atata;

internal sealed class CharExpressionValueStringifier : IExpressionValueStringifier
{
    bool IExpressionValueStringifier.CanHandle(Type type) =>
        type == typeof(char);

    string IExpressionValueStringifier.Handle(object value) =>
        $"'{value}'";
}
