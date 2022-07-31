using NUnit.Framework;

namespace Atata.IntegrationTests.Controls
{
    public class ContentEditorTests : UITestFixture
    {
        [Test]
        public void ContentEditor()
        {
            var sut = Go.To<SummernotePage>().EditorAsContentEditor;

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
}
