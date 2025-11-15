namespace Atata;

internal sealed class StringExpressionValueStringifier : IExpressionValueStringifier
{
    bool IExpressionValueStringifier.CanHandle(Type type) =>
        type == typeof(string);

    string IExpressionValueStringifier.Handle(object value) =>
        $"\"{value}\"";
}
