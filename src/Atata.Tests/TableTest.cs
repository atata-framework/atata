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
                SimpleTable.VerifyExists().
                SimpleTable.VerifyColumns("First Name", "Last Name").
                SimpleTable.FirstRow().Content.VerifyContains("John").
                Do(x => x.SimpleTable.Row("Jack"), x =>
                {
                    x.VerifyExists();
                    x.Content.VerifyContains("Jameson");
                }).
                ComplexTable.Row("Jack", "Jameson").VerifyExists().
                ComplexTable.Row("Jack Jameson").VerifyMissing();
        }

        [Test]
        public void Table_Complex()
        {
            page.
                ComplexTable.VerifyExists().
                ComplexTable.VerifyColumns("First Name", "Last Name").
                ComplexTable.FirstRow().FirstName.VerifyEquals("John").
                Do(x => x.ComplexTable.Row(r => r.FirstName == "Jack"), x =>
                {
                    x.VerifyExists();
                    x.LastName.VerifyEquals("Jameson");
                }).
                ComplexTable.Row("Jack", "Jameson").VerifyExists().
                ComplexTable.Row("Jack Jameson").VerifyMissing();
        }

        [Test]
        public void Table_Navigatable()
        {
            var goToPage = page.
                NavigatableTable.VerifyExists().
                NavigatableTable.Row(r => r.FirstName == "Jack").Click();

            Assert.That(goToPage, Is.Not.Null);
        }
    }
}
