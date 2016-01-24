using System.Threading;

namespace Atata
{
    public class WaitAttribute : TriggerAttribute
    {
        public WaitAttribute(double seconds, TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope applyTo = TriggerScope.Self)
            : base(on, priority, applyTo)
        {
            Seconds = seconds;
        }

        public double Seconds { get; private set; }

        public override void Run(TriggerContext context)
        {
            Thread.Sleep((int)(Seconds * 1000));
        }
    }
}
