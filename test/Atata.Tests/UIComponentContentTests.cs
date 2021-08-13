using System.Collections;
using NUnit.Framework;

namespace Atata.Tests
{
    public class UIComponentContentTests : UITestFixture
    {
        private static IEnumerable ComplexTextTestCaseSource =>
            new[]
            {
                new TestCaseData(ContentSource.Text).Returns("The quick brown fox jumps over the lazy dog."),
                new TestCaseData(ContentSource.TextContent).Returns("The quick brown fox jumps over the lazy dog."),
                new TestCaseData(ContentSource.InnerHtml).Returns("The quick <strong>brown</strong> fox <span><span>jumps</span> over</span> the lazy dog."),
                new TestCaseData(ContentSource.ChildTextNodes).Returns("The quick  fox  the lazy dog."),
                new TestCaseData(ContentSource.ChildTextNodesTrimmed).Returns("The quickfoxthe lazy dog."),
                new TestCaseData(ContentSource.ChildTextNodesTrimmedAndSpaceJoined).Returns("The quick fox the lazy dog."),
                new TestCaseData(ContentSource.FirstChildTextNode).Returns("The quick"),
                new TestCaseData(ContentSource.LastChildTextNode).Returns("the lazy dog.")
            };

        [Test]
        public void Content()
        {
            Go.To<ContentPage>().
                VisibleDiv.Should.Equal("Some text").
                VisibleDiv.Content.Should.Equal("Some text");
        }

        [Test]
        public void Content_Invisible()
        {
            Go.To<ContentPage>().
                HiddenDiv.Should.Not.BeVisible().
                HiddenDiv.Should.BeNull().
                HiddenDiv.Content.Should.BeEmpty().
                HiddenDivUsingTextContent.Should.Equal("Some text").
                HiddenDivUsingTextContent.Content.Should.Equal("Some text").
                HiddenDivUsingInnerHtml.Should.Equal("Some <b>text</b>").
                HiddenDivUsingInnerHtml.Content.Should.Equal("Some <b>text</b>");
        }

        [TestCaseSource(nameof(ComplexTextTestCaseSource))]
        public string Content_UsingGetsContentFromSourceAttribute(ContentSource contentSource)
        {
            return Go.To<ContentPage>().
                GetComplexText(contentSource).Value;
        }

        [TestCaseSource(nameof(ComplexTextTestCaseSource))]
        public string GetContent(ContentSource contentSource)
        {
            return Go.To<ContentPage>().
                ComplexTextControl.GetContent(contentSource);
        }
    }
}
