using System;

namespace Atata
{
    /// <summary>
    /// Indicates that component log messages should be written with <see cref="LogLevel.Trace"/> log level instead of <see cref="LogLevel.Info"/>.
    /// Attribute is useful for sub-controls of complex controls to keep info log cleaner by skipping sub-control interactional log messages.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Property)]
    public class TraceLogAttribute : Attribute
    {
    }
}
