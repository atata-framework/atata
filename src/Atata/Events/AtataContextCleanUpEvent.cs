namespace Atata
{
    /// <summary>
    /// Represents an event that occurs when <see cref="AtataContext"/> is cleaning up.
    /// </summary>
    public class AtataContextCleanUpEvent
    {
        public AtataContextCleanUpEvent(AtataContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        public AtataContext Context { get; }
    }
}
