namespace Atata.IntegrationTests.Bahaviors;

public sealed class ValueClearBehaviorAttributeTests : WebDriverSessionTestSuite
{
    private static TestCaseData[] Source =>
    [
        new TestCaseData(new ClearsValueUsingClearMethodAttribute())
            .SetArgDisplayNames(nameof(ClearsValueUsingClearMethodAttribute)),
        new TestCaseData(new ClearsValueUsingCtrlADeleteKeysAttribute())
            .SetArgDisplayNames(nameof(ClearsValueUsingCtrlADeleteKeysAttribute)),
        new TestCaseData(new ClearsValueUsingHomeShiftEndDeleteKeysAttribute())
            .SetArgDisplayNames(nameof(ClearsValueUsingHomeShiftEndDeleteKeysAttribute)),
        new TestCaseData(new ClearsValueUsingShiftHomeDeleteKeysAttribute())
            .SetArgDisplayNames(nameof(ClearsValueUsingShiftHomeDeleteKeysAttribute)),
        new TestCaseData(new ClearsValueUsingScriptAttribute { IncludeFocusScript = true })
            .SetArgDisplayNames($"{nameof(ClearsValueUsingScriptAttribute)} {{ IncludeFocusScript = true }}"),
        new TestCaseData(new ClearsValueUsingScriptAttribute { IncludeFocusScript = false })
            .SetArgDisplayNames($"{nameof(ClearsValueUsingScriptAttribute)} {{ IncludeFocusScript = false }}"),
        new TestCaseData(new ClearsValueUsingClearMethodOrScriptAttribute { IncludeFocusScript = true })
            .SetArgDisplayNames($"{nameof(ClearsValueUsingClearMethodOrScriptAttribute)} {{ IncludeFocusScript = true }}"),
        new TestCaseData(new ClearsValueUsingClearMethodOrScriptAttribute { IncludeFocusScript = false })
            .SetArgDisplayNames($"{nameof(ClearsValueUsingClearMethodOrScriptAttribute)} {{ IncludeFocusScript = false }}")
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
