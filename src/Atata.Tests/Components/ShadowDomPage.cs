namespace Atata.Tests
{
    using _ = ShadowDomPage;

    [Url("shadowdom")]
    [VerifyTitle("Shadow DOM")]

    [FindById("shadow-container-2", As = FindAs.ShadowHost, TargetName = nameof(Shadow2_1_1_1_AtDifferentLevels))]

    [FindById("shadow-container-2-1", As = FindAs.ShadowHost, Layer = 1, TargetName = nameof(Shadow2_1_1_1_AtDifferentLevelsWithSetLayers))]

    [FindByXPath("div[span[text() = 'Shadow 2.1']]", As = FindAs.Parent, Layer = 1, TargetName = nameof(Shadow2_1_1_1_MixedAtDifferentLevelsWithSetLayers))]
    [FindById("shadow-container-2-1", As = FindAs.ShadowHost, Layer = 1, TargetName = nameof(Shadow2_1_1_1_MixedAtDifferentLevelsWithSetLayers))]
    public class ShadowDomPage : Page<_>
    {
        [FindById("shadow-container-1", As = FindAs.ShadowHost)]
        [ControlDefinition("p")]
        public ControlList<Text<_>, _> Shadow1Paragraphs { get; private set; }

        [FindById(As = FindAs.ShadowHost)]
        [ControlDefinition("p")]
        public ControlList<Text<_>, _> ShadowContainer1 { get; private set; }

        [FindById(As = FindAs.ShadowHost)]
        [Term("shadow-container-1")]
        [ControlDefinition("p")]
        public ControlList<Text<_>, _> ShadowContainer1UsingTerm { get; private set; }

        [FindById("shadow-container-1", As = FindAs.ShadowHost)]
        [FindFirst(Visibility = Visibility.Any)]
        public Text<_> Shadow1_0 { get; private set; }

        [FindById("shadow-container-1", As = FindAs.ShadowHost)]
        [ControlDefinition("p")]
        public Text<_> Shadow1_1 { get; private set; }

        [FindById("shadow-container-1", As = FindAs.ShadowHost)]
        [FindByIndex(2)]
        public Text<_> Shadow1_2 { get; private set; }

        [FindById("shadow-container-2", As = FindAs.ShadowHost)]
        [FindById("shadow-container-2-1", As = FindAs.ShadowHost)]
        [FindByCss("strong")]
        public Text<_> Shadow2_1_1 { get; private set; }

        [FindById("shadow-container-2", As = FindAs.ShadowHost)]
        [FindByCss("#shadow-container-2-1", As = FindAs.ShadowHost)]
        [FindById("shadow-container-2-1-1", As = FindAs.ShadowHost)]
        public Text<_> Shadow2_1_1_1 { get; private set; }

        // The find attribute of first layer is declared at component level.
        // The find attribute of second layer is declared at page object level.
        [FindById("shadow-container-2-1", As = FindAs.ShadowHost)]
        public Shadow2Control Shadow2_1_1_1_AtDifferentLevels { get; private set; }

        // The find attribute of second layer is declared at page object level.
        [FindById("shadow-container-2-1-1", As = FindAs.ShadowHost, Layer = 2)]
        [FindById("shadow-container-2", As = FindAs.ShadowHost, Layer = 0)]
        public Text<_> Shadow2_1_1_1_AtDifferentLevelsWithSetLayers { get; private set; }

        // The find attribute of second layer is declared at page object level.
        [FindById("shadow-container-2-1-1", As = FindAs.ShadowHost, Layer = 2)]
        [FindById("shadow-container-1", As = FindAs.Sibling, Layer = 0)]
        [FindById("shadow-container-2", As = FindAs.ShadowHost, Layer = 0)]
        [FindByXPath("span", As = FindAs.Ancestor, Layer = 3)]
        [FindFirst(OuterXPath = "following-sibling::")]
        public Text<_> Shadow2_1_1_1_MixedAtDifferentLevelsWithSetLayers { get; private set; }

        [FindById("shadow-container-3", As = FindAs.ShadowHost)]
        [FindByName("bool-options")]
        public RadioButtonList<string, _> YesNoRadios { get; private set; }

        [FindById("shadow-container-2-1-1", As = FindAs.ShadowHost)]
        public class Shadow2Control : Control<_>
        {
        }
    }
}
