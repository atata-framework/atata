using NUnit.Framework;

namespace Atata.Tests
{
    [TestFixture]
    public class TableTest : TestBase
    {
        private TablePage page;

        protected override void OnSetUp()
        {
            page = Go.To<TablePage>();
        }

        [Test]
        public void Table_Simple()
        {
            page.
                SimpleTable.VerifyColumns("First Name", "Last Name").
                SimpleTable.FirstRow().VerifyContent("John", TermMatch.Contains).
                Do(x => x.SimpleTable.Row("Jack"), x =>
                {
                    x.VerifyExists();
                    x.VerifyContent("Jameson", TermMatch.Contains);
                });
        }

        [Test]
        public void Table_Complex()
        {
            page.
                ComplexTable.VerifyColumns("First Name", "Last Name").
                ComplexTable.FirstRow().FirstName.VerifyEquals("John").
                Do(x => x.ComplexTable.Row(r => r.FirstName == "Jack"), x =>
                {
                    x.VerifyExists();
                    x.LastName.VerifyEquals("Jameson");
                });
        }
    }
}
