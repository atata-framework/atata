#nullable enable

namespace Atata;

internal class LiteralExpressionPart
{
    private readonly StringBuilder _builder = new();

    public void Append(string value)
        => _builder.Append(value);

    public void Append(char value)
        => _builder.Append(value);

    public override string ToString() =>
        _builder.ToString();
}
