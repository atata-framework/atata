namespace Atata;

internal sealed class DecimalExpressionValueStringifier : IExpressionValueStringifier
{
    bool IExpressionValueStringifier.CanHandle(Type type) =>
        type == typeof(decimal);

    string IExpressionValueStringifier.Handle(object value) =>
        ((decimal)value).ToString(null, CultureInfo.InvariantCulture);
}
