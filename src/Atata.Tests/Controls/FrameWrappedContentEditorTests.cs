using NUnit.Framework;

namespace Atata.Tests.Controls
{
    public class FrameWrappedContentEditorTests : UITestFixture
    {
        [Test]
        public void FrameWrappedContentEditor()
        {
            var sut = Go.To<TinyMcePage>().EditorAsFrameWrappedContentEditor;

            sut.Should.BeEnabled();
            sut.Should.Not.BeReadOnly();

            sut.Set("Abc");
            sut.Should.Equal("Abc");

            sut.Set("Def");
            sut.Should.Equal("Def");

            sut.Clear();
            sut.Should.BeNull();
        }
    }
}
