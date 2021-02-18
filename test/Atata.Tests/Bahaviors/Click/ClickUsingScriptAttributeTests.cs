using NUnit.Framework;

namespace Atata.Tests.Bahaviors
{
    public class ClickUsingScriptAttributeTests : UITestFixture
    {
        [Test]
        public void ClickUsingScriptAttribute_Execute()
        {
            var cell = Go.To<ClickPage>()
                .ClickableCellsTable.Rows[1].Cells[2];

            cell.Metadata.Push(new ClickUsingScriptAttribute());

            cell.Click();

            cell.Should.Equal(1);
        }
    }
}
