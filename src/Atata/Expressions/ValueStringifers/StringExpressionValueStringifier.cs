using System;

namespace Atata
{
    internal class StringExpressionValueStringifier : IExpressionValueStringifier
    {
        public bool CanHandle(Type type) =>
            type == typeof(string);

        public string Handle(object value) =>
            $"\"{value}\"";
    }
}
