using System.Collections.Generic;
using System.Text;

namespace Atata
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendIf(this StringBuilder builder, bool shouldAppend, string message)
        {
            if (shouldAppend)
                builder.Append(message);
            return builder;
        }

        public static StringBuilder AppendJoined(this StringBuilder builder, string separator, IEnumerable<string> values) =>
            builder.Append(string.Join(separator, values));
    }
}
