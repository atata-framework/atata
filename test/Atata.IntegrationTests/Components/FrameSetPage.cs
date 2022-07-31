namespace Atata.IntegrationTests
{
    using _ = FrameSetPage;

    [Url("frameset")]
    [VerifyTitle(TermCase.Pascal)]
    public class FrameSetPage : FrameSetPage<_>
    {
        [FindFirst]
        public Frame<FrameInner1Page, _> Frame1 { get; private set; }

        [FindById]
        public Frame<FrameInner2Page, _> Frame2 { get; private set; }

        [FindById]
        public Frame<FrameInner1Page, _> Frame3 { get; private set; }
    }
}
