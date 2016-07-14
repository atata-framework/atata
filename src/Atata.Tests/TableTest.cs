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
                SimpleTable.Should.Exist().
                SimpleTable.RowCount.Should.Equal(4).
                SimpleTable.VerifyColumns("First Name", "Last Name").
                SimpleTable.FirstRow().Content.Should.Contain("John").
                Do(x => x.SimpleTable.Row("Jack"), x =>
                {
                    x.Should.Exist();
                    x.Content.Should.Contain("Jameson");
                }).
                SimpleTable.Row("Jack", "Jameson").Should.Exist().
                SimpleTable.Row("Jack Jameson").Should.Not.Exist();
        }

        [Test]
        public void Table_Complex()
        {
            page.
                ComplexTable.Should.Exist().
                ComplexTable.RowCount.Should.Equal(4).
                ComplexTable.VerifyColumns("First Name", "Last Name").
                ComplexTable.FirstRow().FirstName.Should.Equal("John").
                Do(x => x.ComplexTable.Row(r => r.FirstName == "Jack"), x =>
                {
                    x.Should.Exist();
                    x.LastName.Should.Equal("Jameson");
                }).
                ComplexTable.Row("Jack", "Jameson").Should.Exist().
                ComplexTable.Row("Jack Jameson").Should.Not.Exist();
        }

        [Test]
        public void Table_Navigatable()
        {
            var goToPage = page.
                NavigatableTable.Should.Exist().
                NavigatableTable.RowCount.Should.Equal(4).
                NavigatableTable.Row(r => r.FirstName == "Jack").Click();

            Assert.That(goToPage, Is.Not.Null);
        }

        [Test]
        public void Table_ByIndex()
        {
            page.
                CountryTable.Should.Exist().
                CountryTable.RowCount.Should.Equal(3).
                CountryTable.FirstRow().Capital.Should.Equal("London").
                Do(x => x.CountryTable.Row(r => r.Capital == "Paris"), x =>
                {
                    x.Should.Exist();
                    x.Country.Should.Equal("France");
                }).
                CountryTable.Row("Germany", "Berlin").Should.Exist();
        }

        [Test]
        public void Table_ByColumnIndex()
        {
            page.
                CountryByColumnIndexTable.Should.Exist().
                CountryByColumnIndexTable.RowCount.Should.Equal(3).
                CountryByColumnIndexTable.FirstRow().CapitalName.Should.Equal("London").
                Do(x => x.CountryByColumnIndexTable.Row(r => r.CapitalName == "Paris"), x =>
                {
                    x.Should.Exist();
                    x.CountryName.Should.Equal("France");
                }).
                CountryByColumnIndexTable.Row("Germany", "Berlin").Should.Exist();
        }
    }
}
