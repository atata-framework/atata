using NUnit.Framework;

namespace Atata.Tests
{
    public class FrameTest : AutoTest
    {
        private FramePage page;

        protected override void OnSetUp()
        {
            page = Go.To<FramePage>();
        }

        [Test]
        public void Frame_PageObject_NavigateViaGo()
        {
            page.Header.Should.Equal("Frame");

            Go.To<FrameInnerPage>().
                Header.Should.Equal("Frame Inner").
                TextInput.Set("abc");

            Go.To<FramePage>(navigate: false).
                Header.Should.Equal("Frame");

            Go.To<FrameInnerPage>().
                Header.Should.Equal("Frame Inner").
                TextInput.Should.Equal("abc");

            // TODO: Remove this line. Test will fail.
            Go.To<FramePage>(navigate: false);
        }
    }
}
