namespace Atata.IntegrationTests.Bahaviors;

public sealed class ValueSetBehaviorAttributeTests : WebDriverSessionTestSuite
{
    private const string InitialValue = "abc";

    private const string SetValue = "def";

    private static TestCaseData[] Source =>
    [
        new TestCaseData(new SetsValueUsingClearAndTypeBehaviorsAttribute())
            .SetArgDisplayNames(nameof(SetsValueUsingClearAndTypeBehaviorsAttribute))
            .Returns(SetValue),
        new TestCaseData(new SetsValueUsingClearAndSendKeysAttribute())
            .SetArgDisplayNames(nameof(SetsValueUsingClearAndSendKeysAttribute))
            .Returns(SetValue),
        new TestCaseData(new SetsValueUsingScriptAttribute { IncludeFocusScript = true })
            .SetArgDisplayNames($"{nameof(SetsValueUsingScriptAttribute)} {{ IncludeFocusScript = true }}")
            .Returns(SetValue),
        new TestCaseData(new SetsValueUsingScriptAttribute { IncludeFocusScript = false })
            .SetArgDisplayNames($"{nameof(SetsValueUsingScriptAttribute)} {{ IncludeFocusScript = false }}")
            .Returns(SetValue),
        new TestCaseData(new SetsValueUsingCharByCharTypingAttribute())
            .SetArgDisplayNames(nameof(SetsValueUsingCharByCharTypingAttribute))
            .Returns(InitialValue + SetValue),
        new TestCaseData(new SetsValueUsingSendKeysAttribute())
            .SetArgDisplayNames(nameof(SetsValueUsingSendKeysAttribute))
            .Returns(InitialValue + SetValue)
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
