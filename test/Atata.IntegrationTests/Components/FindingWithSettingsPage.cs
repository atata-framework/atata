namespace Atata.IntegrationTests;

using _ = FindingWithSettingsPage;

[Url("finding")]
[Name("Finding")]
[VerifyTitle]
[VerifyH1]
[FindByValue(TargetType = typeof(RadioButton<>))]
[FindFirst(TargetType = typeof(RadioFieldSet), TargetParentType = typeof(_))]
[TermFindSettings(Case = TermCase.Pascal, TargetType = typeof(RadioButton<>), TargetAttributeType = typeof(FindByValueAttribute))]
[FindSettings(OuterXPath = "unknown", TargetType = typeof(RadioButton<>))]
[FindSettings(OuterXPath = ".//", TargetType = typeof(RadioButton<>), TargetName = nameof(OptionB))]
[FindSettings(OuterXPath = null, TargetName = nameof(OptionC))]
public class FindingWithSettingsPage : Page<_>
{
    public RadioButton<_> OptionA { get; private set; }

    public RadioButton<_> OptionB { get; private set; }

    public RadioButton<_> OptionC { get; private set; }

    public RadioButton<_> OptionD { get; private set; }

    public RadioFieldSet RadioSet { get; private set; }

    [FindByDescriptionTerm]
    [FindByValue(TargetType = typeof(RadioButton<>))]
    [TermFindSettings(Case = TermCase.Pascal, TargetAttributeType = typeof(FindByValueAttribute), TargetAnyType = true)]
    [FindSettings(Visibility = Visibility.Hidden, TargetType = typeof(Field<,>))]
    [FindSettings(Visibility = Visibility.Visible, TargetTypes = [typeof(Field<,>), typeof(Label<>)], TargetNames = [nameof(OptionB), nameof(OptionD), "Missing"])]
    public class RadioFieldSet : Control<_>
    {
        public RadioButton<_> OptionA { get; private set; }

        public RadioButton<_> OptionB { get; private set; }

        public RadioButton<_> OptionC { get; private set; }

        public RadioButton<_> OptionD { get; private set; }
    }
}
