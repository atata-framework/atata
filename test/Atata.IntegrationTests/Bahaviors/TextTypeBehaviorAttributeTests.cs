namespace Atata.IntegrationTests.Bahaviors;

public class TextTypeBehaviorAttributeTests : WebDriverSessionTestSuite
{
    private const string InitialValue = "abc";

    private const string SetValue = "def";

    private const string ConcatValue = InitialValue + SetValue;

    private static TestCaseData[] Source =>
    [
        new TestCaseData(new TypesTextUsingSendKeysAttribute()).Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingFocusBehaviorAndSendKeysAttribute()).Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingScriptAttribute()).Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingSendKeysCharByCharAttribute()).Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingFocusBehaviorAndSendKeysCharByCharAttribute()).Returns(ConcatValue)
    ];

    [TestCaseSource(nameof(Source))]
    public string Execute(TextTypeBehaviorAttribute behavior)
    {
        var sut = Go.To<InputPage>().TextInput;
        sut.Set(InitialValue);

        sut.Metadata.Push(behavior);

        sut.Type(SetValue);

        return sut.Value;
    }
}
