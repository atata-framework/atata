using System;

namespace Atata
{
    internal class PrimitiveExpressionValueStringifier : IExpressionValueStringifier
    {
        public bool CanHandle(Type type) =>
            type.IsPrimitive;

        public string Handle(object value) =>
            value.ToString();
    }
}
