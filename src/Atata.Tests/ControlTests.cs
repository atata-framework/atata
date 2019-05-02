using System.Drawing;
using NUnit.Framework;

namespace Atata.Tests
{
    public class ControlTests : UITestFixture
    {
        protected override bool ReuseDriver => false;

        [Test]
        public void Control_DragAndDrop_UsingDomEvents()
        {
            Go.To<DragAndDropPage>().
                DropContainer.Items.Should.BeEmpty().
                DragItems.Items.Should.HaveCount(2).
                DragItems[x => x.Content == "Drag item 1"].DragAndDropTo(x => x.DropContainer).
                DragItems[0].DragAndDropTo(x => x.DropContainer).
                DropContainer.Items.Should.HaveCount(2).
                DragItems.Items.Should.BeEmpty().
                DropContainer[1].Content.Should.Equal("Drag item 2");
        }

        [Test]
        public void Control_ScrollTo_UsingMoveToElement()
        {
            AtataContext.Current.Driver.Manage().Window.Size = new Size(400, 400);

            Go.To<BasicControlsPage>().
                OptionBWithScrollUsingMoveToElement.ScrollTo();

            long yOffset = (long)AtataContext.Current.Driver.ExecuteScript("return window.pageYOffset;");

            Assert.That(yOffset, Is.GreaterThan(200));
        }

        [Test]
        public void Control_ScrollTo_UsingScrollIntoView()
        {
            AtataContext.Current.Driver.Manage().Window.Size = new Size(400, 400);

            Go.To<BasicControlsPage>().
                OptionBWithScrollUsingScrollIntoView.ScrollTo();

            long yOffset = (long)AtataContext.Current.Driver.ExecuteScript("return window.pageYOffset;");

            Assert.That(yOffset, Is.GreaterThan(200));
        }

        [Test]
        public void Control_ClickAndGo()
        {
            Go.To<GoTo1Page>().
                GoTo2Control.ClickAndGo<GoTo2Page>();
        }
    }
}
