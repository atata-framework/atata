namespace Atata.IntegrationTests.Controls;

public class FrameWrappedContentEditorTests : WebDriverSessionTestSuite
{
    [Test]
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
