#nullable enable

namespace Atata;

internal class EnumExpressionValueStringifier : IExpressionValueStringifier
{
    public bool CanHandle(Type type) =>
        type.IsEnum;

    public string Handle(object value) =>
        ((Enum)value).ToExpressionValueString();
}
