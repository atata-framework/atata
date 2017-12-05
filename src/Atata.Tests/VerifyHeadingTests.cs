using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class VerifyHeadingTests : UITestFixture
    {
        [TestCase(TestName = "Trigger_VerifyH1-6")]
        public void Trigger_VerifyH1_6()
        {
            Go.To<HeadingPage>();

            string[] logMessages = LogEntries.Select(x => x.Message).ToArray();

            string[] expectedLogMessageTexts =
            {
                "Heading 1_2",
                "Heading 2.1.2.1.1.1",
                "Heading 2.2",
                "Verify \"4th\" <h2> heading content should equal \"Heading 2.2\"",
                "Verify \"Heading_2/Heading 2\" <h1> heading should exist"
            };

            foreach (var text in expectedLogMessageTexts)
                Assert.That(logMessages, Has.Some.Contains(text));
        }
    }
}
