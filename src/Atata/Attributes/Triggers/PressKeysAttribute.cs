using OpenQA.Selenium.Interactions;

namespace Atata
{
    public class PressKeysAttribute : TriggerAttribute
    {
        public PressKeysAttribute(string keys = null)
        {
            Keys = keys;
        }

        public string Keys { get; protected set; }

        public override void Run(TriggerContext context)
        {
            if (!string.IsNullOrEmpty(Keys))
            {
                Actions actions = new Actions(context.Driver);
                actions.SendKeys(Keys).Build().Perform();
            }
        }
    }
}
