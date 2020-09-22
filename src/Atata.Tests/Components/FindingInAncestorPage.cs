namespace Atata.Tests
{
    using _ = FindingInAncestorPage;

    [Url("finding")]
    [Name("Finding")]
    [VerifyTitle]
    [VerifyH1]

    [FindByClass("row", As = FindAs.Ancestor, TargetName = nameof(LegendInThreeLayersAtParentAndDeclared))]
    [FindByClass("form-group", As = FindAs.Ancestor, TargetName = nameof(LegendInThreeLayersAtParentAndDeclared))]

    [FindByClass("row", As = FindAs.Ancestor, TargetName = nameof(LegendInThreeLayersAtParentAndDeclaredAndComponent))]
    public class FindingInAncestorPage : Page<_>
    {
        [FindByClass("x-radio-container", As = FindAs.Ancestor)]
        [ControlDefinition("legend")]
        public Text<_> LegendInOneLayer { get; private set; }

        [FindByClass("row", As = FindAs.Ancestor)]
        [FindByClass("form-group", As = FindAs.Ancestor)]
        [FindByClass("x-radio-container", As = FindAs.Ancestor)]
        public Text<_> LegendInThreeLayers { get; private set; }

        [FindByClass("x-radio-container", As = FindAs.Ancestor)]
        public Text<_> LegendInThreeLayersAtParentAndDeclared { get; private set; }

        [FindByClass("form-group", As = FindAs.Ancestor)]
        public CustomControl LegendInThreeLayersAtParentAndDeclaredAndComponent { get; private set; }

        [FindByClass("x-radio-container", As = FindAs.Ancestor)]
        public class CustomControl : Text<_>
        {
        }
    }
}
