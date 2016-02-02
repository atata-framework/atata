using System.Threading;

namespace Atata
{
    public class WaitAttribute : TriggerAttribute
    {
        public WaitAttribute(double seconds, TriggerEvents on = TriggerEvents.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
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
