namespace Atata;

internal sealed class EnumExpressionValueStringifier : IExpressionValueStringifier
{
    bool IExpressionValueStringifier.CanHandle(Type type) =>
        type.IsEnum;

    string IExpressionValueStringifier.Handle(object value) =>
        ((Enum)value).ToExpressionValueString();
}
