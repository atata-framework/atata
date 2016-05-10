using OpenQA.Selenium;

namespace Atata
{
    public class CloseConfirmBoxAttribute : TriggerAttribute
    {
        public CloseConfirmBoxAttribute(bool accept = true, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium, TriggerScope appliesTo = TriggerScope.Self)
            : base(on, priority, appliesTo)
        {
            Accept = accept;
        }

        public bool Accept { get; private set; }

        public override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            IAlert alert = context.Driver.SwitchTo().Alert();

            if (Accept)
                alert.Accept();
            else
                alert.Dismiss();

            context.Driver.SwitchTo().DefaultContent();
        }
    }
}
