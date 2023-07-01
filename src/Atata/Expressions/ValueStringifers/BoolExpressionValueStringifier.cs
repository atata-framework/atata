namespace Atata;

internal class BoolExpressionValueStringifier : IExpressionValueStringifier
{
    public bool CanHandle(Type type) =>
        type == typeof(bool);

    public string Handle(object value) =>
        value.ToString().ToLowerInvariant();
}
