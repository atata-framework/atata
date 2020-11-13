using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class ClickUsingScriptAttributeTests : UITestFixture
    {
        [Test]
        public void ClickUsingScriptAttribute_Execute()
        {
            var cell = Go.To<TablePage>()
                .ClickableCellsTable.Rows[1].Cell3;

            cell.Metadata.Push(new ClickUsingScriptAttribute());

            cell.Click();

            cell.Should.Equal(1);
            cell.Should.Not.BeVisibleInViewPort();
        }
    }
}
