namespace Atata.Tests
{
    using _ = ShadowHostPage;

    [FindById("shadow-container-1", As = FindAs.ShadowHost, TargetAnyType = true)]
    public class ShadowHostPage : Page<_>
    {
        [ControlDefinition("p")]
        public ControlList<Text<_>, _> Paragraphs { get; private set; }

        [FindByIndex(1)]
        [ControlDefinition("p")]
        public Text<_> Paragraph2 { get; private set; }
    }
}
