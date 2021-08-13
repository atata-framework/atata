using System.Drawing;
using NUnit.Framework;

namespace Atata.Tests
{
    public class ControlTests : UITestFixture
    {
        protected override bool ReuseDriver => false;

        [Test]
        public void DragAndDrop_With_DragsAndDropsUsingDomEvents()
        {
            Go.To<DragAndDropPage>()
                .DropContainer.Items.Should.BeEmpty()
                .DragItems.Items.Should.HaveCount(2)
                .DragItems[x => x.Content == "Drag item 1"].DragAndDropTo(x => x.DropContainer)
                .DragItems[0].DragAndDropTo(x => x.DropContainer)
                .DropContainer.Items.Should.HaveCount(2)
                .DragItems.Items.Should.BeEmpty()
                .DropContainer[1].Content.Should.Equal("Drag item 2");
        }

        [Test]
        public void ScrollTo_With_ScrollsUsingActionsAttribute()
        {
            AtataContext.Current.Driver.Manage().Window.Size = new Size(400, 400);

            Go.To<BasicControlsPage>()
                .OptionBWithScrollUsingActions.ScrollTo();

            long yOffset = (long)AtataContext.Current.Driver.ExecuteScript("return window.pageYOffset;");

            Assert.That(yOffset, Is.GreaterThan(200));
        }

        [Test]
        public void ScrollTo_With_ScrollsUsingScriptAttribute()
        {
            AtataContext.Current.Driver.Manage().Window.Size = new Size(400, 400);

            Go.To<BasicControlsPage>()
                .OptionBWithScrollUsingScript.ScrollTo();

            long yOffset = (long)AtataContext.Current.Driver.ExecuteScript("return window.pageYOffset;");

            Assert.That(yOffset, Is.GreaterThan(200));
        }

        [Test]
        public void ClickAndGo()
        {
            Go.To<GoTo1Page>()
                .GoTo2Control.ClickAndGo<GoTo2Page>();
        }
    }
}
