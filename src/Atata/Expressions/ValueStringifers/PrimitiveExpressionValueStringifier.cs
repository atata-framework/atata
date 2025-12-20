namespace Atata;

internal sealed class PrimitiveExpressionValueStringifier : IExpressionValueStringifier
{
    bool IExpressionValueStringifier.CanHandle(Type type) =>
        type.IsPrimitive;

    string IExpressionValueStringifier.Handle(object value) =>
        value is IFormattable formattableValue
            ? formattableValue.ToString(null, CultureInfo.InvariantCulture)
            : value.ToString() ?? string.Empty;
}
