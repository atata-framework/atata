namespace Atata.IntegrationTests.Bahaviors;

public sealed class TextTypeBehaviorAttributeTests : WebDriverSessionTestSuite
{
    private const string InitialValue = "abc";

    private const string SetValue = "def";

    private const string ConcatValue = InitialValue + SetValue;

    private static TestCaseData[] Source =>
    [
        new TestCaseData(new TypesTextUsingSendKeysAttribute())
            .SetArgDisplayNames(nameof(TypesTextUsingSendKeysAttribute))
            .Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingFocusBehaviorAndSendKeysAttribute())
            .SetArgDisplayNames(nameof(TypesTextUsingFocusBehaviorAndSendKeysAttribute))
            .Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingScriptAttribute { IncludeFocusScript = true })
            .SetArgDisplayNames($"{nameof(TypesTextUsingScriptAttribute)} {{ IncludeFocusScript = true }}")
            .Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingScriptAttribute { IncludeFocusScript = false })
            .SetArgDisplayNames($"{nameof(TypesTextUsingScriptAttribute)} {{ IncludeFocusScript = false }}")
            .Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingSendKeysCharByCharAttribute())
            .SetArgDisplayNames(nameof(TypesTextUsingSendKeysCharByCharAttribute))
            .Returns(ConcatValue),
        new TestCaseData(new TypesTextUsingFocusBehaviorAndSendKeysCharByCharAttribute())
            .SetArgDisplayNames(nameof(TypesTextUsingFocusBehaviorAndSendKeysCharByCharAttribute))
            .Returns(ConcatValue)
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
