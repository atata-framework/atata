namespace Atata.Tests
{
    using _ = ShadowHostPopupWindow;

    [PageObjectDefinition("div[@id='shadow-container-1']")]
    [UseParentScope(As = FindAs.ShadowHost, TargetAnyType = true)]
    public class ShadowHostPopupWindow : PopupWindow<_>
    {
        [ControlDefinition("p")]
        public ControlList<Text<_>, _> Paragraphs { get; private set; }

        [FindByIndex(1)]
        [ControlDefinition("p")]
        public Text<_> Paragraph2 { get; private set; }
    }
}
