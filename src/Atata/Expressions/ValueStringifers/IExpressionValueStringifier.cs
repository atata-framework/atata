#nullable enable

namespace Atata;

internal interface IExpressionValueStringifier
{
    bool CanHandle(Type type);

    string Handle(object value);
}
