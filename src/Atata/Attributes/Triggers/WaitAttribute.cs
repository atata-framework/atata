using System.Threading;

namespace Atata
{
    public class WaitAttribute : TriggerAttribute
    {
        public WaitAttribute(double seconds, TriggerEvent on = TriggerEvent.AfterAnyAction, TriggerPriority priority = TriggerPriority.Medium, TriggerScope scope = TriggerScope.Self)
            : base(on, priority, scope)
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
