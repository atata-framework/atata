namespace Atata
{
    public class ScrollUpAttribute : TriggerAttribute
    {
        public override void Run(TriggerContext context)
        {
            context.Driver.ExecuteScript("scroll(0,0);");
        }
    }
}
