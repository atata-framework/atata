namespace Atata.IntegrationTests.Controls;

public class FrameWrappedContentEditorTests : UITestFixture
{
    [Test]
    [Ignore("Started to fail on 2026-06-12 due to the latest Chrome.")] // TODO: Investigate and fix the issue.
    public void Interact()
    {
        var sut = Go.To<TinyMcePage>().EditorAsFrameWrappedContentEditor;

        sut.Should.BeEnabled();
        sut.Should.Not.BeReadOnly();

        sut.Set("Abc");
        sut.Should.Equal("Abc");

        sut.Set("Def");
        sut.Should.Equal("Def");

        sut.Type("Ghi");
        sut.Should.Equal("DefGhi");

        sut.Clear();
        sut.Should.BeEmpty();
    }
}
