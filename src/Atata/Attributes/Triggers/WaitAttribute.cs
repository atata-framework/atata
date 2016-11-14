using System.Threading;

namespace Atata
{
    /// <summary>
    /// Specifies the waiting period in seconds. By default occurs after any action.
    /// </summary>
    public class WaitAttribute : TriggerAttribute
    {
        public WaitAttribute(double seconds, TriggerEvents on = TriggerEvents.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
            Seconds = seconds;
        }

        public double Seconds { get; private set; }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            Thread.Sleep((int)(Seconds * 1000));
        }
    }
}
