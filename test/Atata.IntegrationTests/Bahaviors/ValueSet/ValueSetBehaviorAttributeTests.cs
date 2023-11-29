namespace Atata.IntegrationTests.Bahaviors;

public class ValueSetBehaviorAttributeTests : UITestFixture
{
    private const string InitialValue = "abc";

    private const string SetValue = "def";

    private static TestCaseData[] Source =>
    [
        new TestCaseData(new SetsValueUsingClearAndTypeBehaviorsAttribute()).Returns(SetValue),
        new TestCaseData(new SetsValueUsingClearAndSendKeysAttribute()).Returns(SetValue),
        new TestCaseData(new SetsValueUsingScriptAttribute()).Returns(SetValue),
        new TestCaseData(new SetsValueUsingCharByCharTypingAttribute()).Returns(InitialValue + SetValue),
        new TestCaseData(new SetsValueUsingSendKeysAttribute()).Returns(InitialValue + SetValue)
    ];

    [TestCaseSource(nameof(Source))]
    public string Execute(ValueSetBehaviorAttribute behavior)
    {
        var sut = Go.To<InputPage>().TextInput;
        sut.Set(InitialValue);

        sut.Metadata.Push(behavior);

        sut.Set(SetValue);

        return sut.Value;
    }
}
