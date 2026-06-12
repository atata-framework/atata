namespace Atata.IntegrationTests.Controls;

public class FrameWrappedContentEditorTests : WebDriverSessionTestSuite
{
    [Test]
    [Ignore("Started to fail on 2026-06-12 due to the latest Chrome.")] // TODO: Investigate and fix the issue.
    public void Interact()
    {
        var sut = Go.To<TinyMcePage>().EditorAsFrameWrappedContentEditor;

        sut.Should.BeEnabled();
        sut.Should.Not.BeReadOnly();

        sut.Set("Abc");
        sut.Should.Be("Abc");

        sut.Set("Def");
        sut.Should.Be("Def");

        sut.Type("Ghi");
        sut.Should.Be("DefGhi");

        sut.Clear();
        sut.Should.BeEmpty();
    }
}
