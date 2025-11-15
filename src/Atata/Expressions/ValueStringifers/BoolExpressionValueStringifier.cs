namespace Atata;

internal sealed class BoolExpressionValueStringifier : IExpressionValueStringifier
{
    bool IExpressionValueStringifier.CanHandle(Type type) =>
        type == typeof(bool);

    string IExpressionValueStringifier.Handle(object value) =>
        value is true ? "true" : "false";
}
