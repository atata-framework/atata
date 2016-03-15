using Humanizer;

namespace Atata
{
    public abstract class Control<TOwner> : UIComponent<TOwner>, IClickable
        where TOwner : PageObject<TOwner>
    {
        protected Control()
        {
        }

        public TOwner Click()
        {
            RunTriggers(TriggerEvents.BeforeClick);
            Log.StartClickingSection(ComponentName);

            Scope.Click();

            Log.EndSection();
            RunTriggers(TriggerEvents.AfterClick);

            return Owner;
        }

        void IClickable.Click()
        {
            Click();
        }

        public TOwner VerifyEnabled()
        {
            Log.StartVerificationSection("{0} component is enabled", ComponentName);
            Assert.That(IsEnabled(), "Expected {0} component to be enabled", ComponentName);
            Log.EndSection();
            return Owner;
        }

        public TOwner VerifyDisabled()
        {
            Log.StartVerificationSection("{0} component is disabled", ComponentName);
            Assert.That(!IsEnabled(), "Expected {0} component to be disabled", ComponentName);
            Log.EndSection();
            return Owner;
        }

        public virtual bool IsEnabled()
        {
            return Scope.Enabled;
        }

        public TOwner VerifyContent(string content, TermMatch match = TermMatch.Equals)
        {
            string matchActionText = match.ToSentenceString();
            Log.StartVerificationSection("{0} component text {1} '{2}'", ComponentName, matchActionText, content);

            var matchPredicate = match.GetPredicate();
            string actualText = Scope.Text;
            bool doesMatch = matchPredicate(actualText, content);
            string errorMessage = ExceptionFactory.BuildAssertionErrorMessage(
                "String that {0} '{1}'".FormatWith(matchActionText, content),
                actualText,
                "{0} component text doesn't match criteria", ComponentName);
            Assert.That(doesMatch, errorMessage);

            Log.EndSection();
            return Owner;
        }
    }
}
