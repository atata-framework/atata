namespace Atata.IntegrationTests.Bahaviors;

public class ValueClearBehaviorAttributeTests : WebDriverSessionTestSuite
{
    private static TestCaseData[] Source =>
    [
        new TestCaseData(new ClearsValueUsingClearMethodAttribute()),
        new TestCaseData(new ClearsValueUsingCtrlADeleteKeysAttribute()),
        new TestCaseData(new ClearsValueUsingHomeShiftEndDeleteKeysAttribute()),
        new TestCaseData(new ClearsValueUsingShiftHomeDeleteKeysAttribute()),
        new TestCaseData(new ClearsValueUsingScriptAttribute()),
        new TestCaseData(new ClearsValueUsingClearMethodOrScriptAttribute())
    ];

    [TestCaseSource(nameof(Source))]
    public void Execute(ValueClearBehaviorAttribute behavior)
    {
        var sut = Go.To<InputPage>().TextInput;
        sut.Set("abc");

        sut.Metadata.Push(behavior);

        sut.Clear();

        sut.Should.BeEmpty();
    }
}
