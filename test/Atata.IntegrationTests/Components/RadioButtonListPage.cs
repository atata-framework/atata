namespace Atata.IntegrationTests;

using _ = RadioButtonListPage;

[Url("radiobuttonlist")]
[VerifyTitle(TermCase.Pascal)]
public class RadioButtonListPage : Page<_>
{
    public enum ByLabel
    {
        OptionA,
        OptionB,
        OptionC,
        OptionD
    }

    [TermSettings(TermCase.Title)]
    public enum ByValue
    {
        None,
        [Term(TermCase.Pascal)]
        OptionA,
        [Term(TermCase.Pascal)]
        OptionB,
        [Term("COption", "OptionC")]
        OptionC,
        [Term(TermCase.Pascal)]
        OptionD,
        MissingValue
    }

    [FindByName("radio-options")]
    [FindItemByLabel]
    public RadioButtonList<ByLabel?, _> ByNameAndLabel { get; private set; }

    [FindByClass("x-radio-container")]
    [FindItemByValue]
    public RadioButtonList<ByValue, _> ByClassAndValue { get; private set; }

    [FindByCss(".x-radio-container")]
    [FindItemByValue(TermCase.Pascal)]
    public RadioButtonList<ByLabel, _> ByCssAndValue { get; private set; }

    [FindByClass(TermCase.Snake)]
    [FindItemByLabel]
    public RadioButtonList<int?, _> IntegerItems { get; private set; }

    [FindByName]
    [FindItemByLabel]
    [Format("C")]
    public RadioButtonList<decimal?, _> DecimalItems { get; private set; }

    [FindByFieldSet("Bool List")]
    [FindItemByValue]
    public RadioButtonList<bool?, _> BoolItems { get; private set; }

    [FindByFieldSet("Bool List Lowered")]
    [FindItemByValue(TermCase.Lower)]
    public RadioButtonList<bool, _> BoolLoweredItems { get; private set; }

    [FindById]
    [FindItemByParentContent]
    public RadioButtonList<string, _> TextInParentItems { get; private set; }

    [FindById]
    [FindItemByFollowingSiblingContent]
    public RadioButtonList<string, _> TextInFollowingSiblingItems { get; private set; }

    [FindById]
    [FindItemByPrecedingSiblingContent]
    public RadioButtonList<string, _> TextInPrecedingSiblingItems { get; private set; }

    [FindByName(TermCase.Kebab)]
    [FindItemByLabel]
    public RadioButtonList<string, _> VerticalItems { get; private set; }

    [FindByFieldSet("Vertical List")]
    [FindItemByLabel]
    public RadioButtonList<string, _> VerticalItemsByFieldSet { get; private set; }

    public RadioButton<_> OptionA { get; private set; }

    public RadioButton<_> OptionB { get; private set; }

    [FindByFieldSet("Integer List")]
    public ItemsControl<RadioButton<_>, _> IntegerItemsControl { get; private set; }
}
