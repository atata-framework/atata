using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property)]
    public class TraceLogAttribute : Attribute
    {
    }
}
