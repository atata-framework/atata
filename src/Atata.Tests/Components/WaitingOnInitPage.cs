namespace Atata.Tests
{
    using _ = WaitingOnInitPage;

    [Url("WaitingOnInit.html")]
    public class WaitingOnInitPage : Page<_>
    {
        private readonly WaitKind waitKind;

        public WaitingOnInitPage(WaitKind waitKind = WaitKind.WaitForElement)
        {
            this.waitKind = waitKind;
        }

        public enum WaitKind
        {
            WaitForElement,
            VerifyExists,
            VerifyMissing
        }

        [FindByClass]
        public Control<_> LoadingBlock { get; private set; }

        [FindByClass]
        public Text<_> ContentBlock { get; private set; }

        protected override void OnInit()
        {
            switch (waitKind)
            {
                case WaitKind.WaitForElement:
                    Triggers.Add(new WaitForElementAttribute(WaitBy.Class, "content-block", WaitUntil.Visible, TriggerEvents.Init));
                    break;
                case WaitKind.VerifyExists:
                    ContentBlock.Triggers.Add(new VerifyExistsAttribute());
                    break;
                case WaitKind.VerifyMissing:
                    LoadingBlock.Triggers.Add(new VerifyMissingAttribute());
                    break;
            }
        }
    }
}
