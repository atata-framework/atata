using NUnit.Framework;

namespace Atata.Tests
{
    using _ = WaitingOnInitPage;

    [Url(Url)]
    public class WaitingOnInitPage : Page<_>
    {
        public const string Url = "WaitingOnInit.html";

        public enum WaitKind
        {
            None,
            WaitForElement,
            VerifyExists,
            VerifyMissing
        }

        [FindByClass]
        public Control<_> LoadingBlock { get; private set; }

        [FindByClass]
        public Text<_> ContentBlock { get; private set; }

        public WaitKind OnInitWaitKind { get; set; }

        protected override void OnInit()
        {
            if (OnInitWaitKind == WaitKind.WaitForElement)
                Triggers.Add(new WaitForElementAttribute(WaitBy.Class, "content-block", WaitUntil.Visible, TriggerEvents.Init));
            else if (OnInitWaitKind == WaitKind.VerifyExists)
                ContentBlock.Triggers.Add(new VerifyExistsAttribute());
            else if (OnInitWaitKind == WaitKind.VerifyMissing)
                LoadingBlock.Triggers.Add(new VerifyMissingAttribute());
        }

        public _ VerifyContentBlockIsLoaded()
        {
            Assert.That(ContentBlock.GetScope(SearchOptions.UnsafelyAtOnce()).Text, Is.EqualTo("Loaded"));
            return this;
        }
    }
}
