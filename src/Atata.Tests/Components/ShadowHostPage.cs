namespace Atata.Tests
{
    using _ = ShadowHostPage;

    [FindById("shadow-container-1", As = FindAs.ShadowHost, TargetAnyType = true)]
    public class ShadowHostPage : Page<_>
    {
        public ControlList<Text<_>, _> Paragraphs { get; private set; }
    }
}
