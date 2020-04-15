namespace Atata.Tests
{
    using _ = FindingInAncestorPage;

    [Url("finding")]
    [Name("Finding")]
    [VerifyTitle]
    [VerifyH1]
    [FindByClass("row", As = FindAs.Ancestor, TargetName = nameof(LegendInThreeLayersAtDifferentLevels))]
    [FindByClass("form-group", As = FindAs.Ancestor, TargetName = nameof(LegendInThreeLayersAtDifferentLevels))]
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
        public Text<_> LegendInThreeLayersAtDifferentLevels { get; private set; }
    }
}
