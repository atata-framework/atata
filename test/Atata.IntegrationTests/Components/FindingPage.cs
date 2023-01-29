namespace Atata.IntegrationTests;

using _ = FindingPage;

[Url("finding")]
[VerifyTitle]
[VerifyH1]
[FindByValue("OptionC", TargetName = nameof(OptionCAsCustom))]
public class FindingPage : Page<_>
{
    [FindByIndex(2)]
    public RadioButton<_> OptionCByIndex { get; private set; }

    [FindByName("radio-options", Index = 2)]
    public RadioButton<_> OptionCByName { get; private set; }

    [FindByCss("[name='radio-options']", Index = 2)]
    public RadioButton<_> OptionCByCss { get; private set; }

    [FindByXPath("non-existent", "*[@name='radio-options']", Index = 2)]
    public RadioButton<_> OptionCByXPath { get; private set; }

    [FindByXPath("[@name='radio-options']", Index = 2)]
    public RadioButton<_> OptionCByXPathCondition { get; private set; }

    [FindByXPath("@name='radio-options'", Index = 2)]
    public RadioButton<_> OptionCByXPathAttribute { get; private set; }

    [FindByAttribute("name", "radio-options", Index = 2)]
    public RadioButton<_> OptionCByAttribute { get; private set; }

    [FindByClass("radio-options", Index = 2)]
    public RadioButton<_> OptionCByClass { get; private set; }

    [FindByLabel("Option C")]
    public RadioButton<_> OptionCByLabel { get; private set; }

    [FindByLabel(TermMatch.StartsWith, "Option", Index = 2)]
    public RadioButton<_> OptionCByLabelWithIndex { get; private set; }

    [FindLast]
    public RadioButton<_> OptionDAsLast { get; private set; }

    public CustomRadioButton OptionDAsCustom { get; private set; }

    public CustomRadioButton OptionCAsCustom { get; private set; }

    [FindById]
    public TextInput<_> VisibleInput { get; private set; }

    [FindById(Visibility = Visibility.Hidden)]
    public TextInput<_> DisplayNoneInput { get; private set; }

    [FindById("display-none-input", Visibility = Visibility.Visible)]
    public TextInput<_> FailDisplayNoneInput { get; private set; }

    [FindById(Visibility = Visibility.Hidden)]
    public TextInput<_> HiddenInput { get; private set; }

    [FindById(Visibility = Visibility.Hidden)]
    public TextInput<_> CollapseInput { get; private set; }

    [FindById(Visibility = Visibility.Hidden)]
    public TextInput<_> Opacity0Input { get; private set; }

    [FindById("opacity-0-input", Visibility = Visibility.Visible)]
    public TextInput<_> FailOpacity0Input { get; private set; }

    [FindById]
    public HiddenInput<_> TypeHiddenInput { get; private set; }

    [FindById("missing")]
    public TextInput<_> MissingInput { get; private set; }

    [ControlDefinition("input[@type='hidden']", Visibility = Visibility.Hidden)]
    [FindById("type-hidden-input")]
    public Input<string, _> TypeHiddenInputWithDeclaredDefinition { get; private set; }

    [FindByCss("[value='OptionC']", OuterXPath = ".//*[@class='x-radio-container']/")]
    public RadioButton<_> OptionCByCssWithOuterXPath { get; private set; }

    [FindByCss("[value='OptionC']", OuterXPath = ".//*[@class='unknown']/")]
    public RadioButton<_> MissingOptionByCssWithOuterXPath { get; private set; }

    [FindByCss("[name='unknown']")]
    public RadioButton<_> MissingOptionByCss { get; private set; }

    [FindByLabel("unknown")]
    public RadioButton<_> MissingOptionByLabel { get; private set; }

    [FindByXPath("*[@name='unknown']")]
    public RadioButton<_> MissingOptionByXPath { get; private set; }

    [FindById("unknown")]
    public RadioButton<_> MissingOptionById { get; private set; }

    [FindByColumnHeader("unknown")]
    public RadioButton<_> MissingOptionByColumnHeader { get; private set; }

    [ControlDefinition("span", ContainingClasses = new[] { "class-a", "class-c" })]
    public Text<_> SpanWithMultipleClasses { get; private set; }

    [ControlDefinition("span", ContainingClasses = new[] { "class-a", "class-d" })]
    public Text<_> MissingSpanWithMultipleClasses { get; private set; }

    [FindByScript("return arguments[0].querySelector('input[type=radio][value=OptionB]')")]
    public RadioButton<_> OptionByScript { get; private set; }

    [FindByScript("return document.querySelectorAll('input[type=radio]')", Index = 2)]
    public RadioButton<_> OptionByScriptWithIndex { get; private set; }

    [FindByScript("return document.querySelectorAll('input[type=radio]')", Index = 9)]
    public RadioButton<_> OptionByScriptWithIncorrectIndex { get; private set; }

    [FindByScript("return document.querySelector('input[type=radio][value=OptionZ]')")]
    public RadioButton<_> OptionByScriptMissing { get; private set; }

    [FindByScript("return document.missingMethod()")]
    public RadioButton<_> OptionByScriptWithInvalidScript { get; private set; }

    [FindByScript("return 'I am not OK.'")]
    public RadioButton<_> OptionByScriptWithIncorrectScriptResult { get; private set; }

    [ControlDefinition(ContainingClass = "custom-control")]
    [FindByDescendantId("custom-control-inner-id")]
    public Control<_> ControlByDescendantId { get; private set; }

    [ControlDefinition(ContainingClass = "custom-control")]
    [FindByDescendantId("custom-control-id")]
    public Control<_> ControlByDescendantIdMissing { get; private set; }

    [FindByLabel("User Name")]
    public TextInput<_> UserNameByLabel { get; private set; }

    [FindByLabel(TermMatch.Contains, "Name", Index = 0)]
    public TextInput<_> UserNameByLabelAndIndex { get; private set; }

    [FindByValue("OptionD")]
    public class CustomRadioButton : RadioButton<_>
    {
    }
}
