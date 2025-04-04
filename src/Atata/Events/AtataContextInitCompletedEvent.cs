namespace Atata;

/// <summary>
/// Represents an event that occurs when <see cref="AtataContext"/> is initialized.
/// </summary>
public sealed class AtataContextInitCompletedEvent : AtataContextEvent
{
    internal AtataContextInitCompletedEvent(AtataContext context)
        : base(context)
    {
    }
}
