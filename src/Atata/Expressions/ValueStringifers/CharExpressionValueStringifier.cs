using System;

namespace Atata
{
    internal class CharExpressionValueStringifier : IExpressionValueStringifier
    {
        public bool CanHandle(Type type) =>
            type == typeof(char);

        public string Handle(object value) =>
            $"'{value}'";
    }
}
