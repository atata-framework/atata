namespace Atata
{
    public class ClickParentAttribute : TriggerAttribute
    {
        public override void Run(TriggerContext context)
        {
            ((IClickable)context.ParentComponent).Click();
        }
    }
}
