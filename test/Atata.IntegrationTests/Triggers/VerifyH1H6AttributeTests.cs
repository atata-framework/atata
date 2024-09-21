namespace Atata.IntegrationTests.Triggers;

public class VerifyH1H6AttributeTests : WebDriverSessionTestSuite
{
    [Test]
    public void Execute()
    {
        Go.To<HeadingPage>();

        string[] logMessages = CurrentLog.GetMessagesSnapshot();

        string[] expectedLogMessageTexts =
        [
            "Heading 1_2",
            "Heading 2.1.2.1.1.1",
            "Heading 2.2",
            "Assert: \"4th\" <h2> heading content should equal \"Heading 2.2\"",
            "Assert: \"Heading_2/Heading 2\" <h1> heading should be present"
        ];

        foreach (var text in expectedLogMessageTexts)
            Assert.That(logMessages, Has.Some.Contains(text));
    }
}
