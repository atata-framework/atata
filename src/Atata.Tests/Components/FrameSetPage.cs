namespace Atata.Tests
{
    using _ = FrameSetPage;

    [Url("FrameSet.html")]
    [VerifyTitle(TermCase.Pascal)]
    public class FrameSetPage : FrameSetPage<_>
    {
        [FindByIndex(0)]
        public Frame<FrameInner1Page, _> Frame1 { get; private set; }

        [FindByIndex(1)]
        public Frame<FrameInner2Page, _> Frame2 { get; private set; }

        [FindByIndex(2)]
        public Frame<FrameInner1Page, _> Frame3 { get; private set; }
    }
}
