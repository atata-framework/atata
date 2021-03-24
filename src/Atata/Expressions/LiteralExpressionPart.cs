using System.Text;

namespace Atata
{
    internal class LiteralExpressionPart
    {
        private readonly StringBuilder builder = new StringBuilder();

        public void Append(string value)
            => builder.Append(value);

        public void Append(char value)
            => builder.Append(value);

        public override string ToString() =>
            builder.ToString();
    }
}
