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
                SimpleTable.Rows.Count.Should.Equal(4).
                SimpleTable.VerifyColumns("First Name", "Last Name").
                SimpleTable.Rows[0].Content.Should.Contain("John").
                Do(x => x.SimpleTable.Rows["Jack"], x =>
                {
                    x.Should.Exist();
                    x.Content.Should.Contain("Jameson");
                }).
                SimpleTable.Rows["Jack", "Jameson"].Should.Exist().
                SimpleTable.Rows["Jack Jameson"].Should.WithoutRetry.Not.Exist();
        }

        [Test]
        public void Table_Complex()
        {
            page.
                ComplexTable.Should.Exist().
                ComplexTable.Rows.Count.Should.Equal(4).
                ComplexTable.VerifyColumns("First Name", "Last Name").
                ComplexTable.Rows[0].FirstName.Should.Equal("John").
                Do(x => x.ComplexTable.Rows[r => r.FirstName == "Jack"], x =>
                {
                    x.Should.Exist();
                    x.LastName.Should.Equal("Jameson");
                }).
                ComplexTable.Rows["Jack", "Jameson"].Should.Exist().
                ComplexTable.Rows["Jack Jameson"].Should.Not.Exist();
        }

        [Test]
        public void Table_Navigatable()
        {
            var goToPage = page.
                NavigatableTable.Should.Exist().
                NavigatableTable.Rows.Count.Should.Equal(4).
                NavigatableTable.Rows[r => r.FirstName == "Jack"].Click();

            Assert.That(goToPage, Is.Not.Null);
        }

        [Test]
        public void Table_ByIndex()
        {
            page.
                CountryTable.Should.Exist().
                CountryTable.Rows.Count.Should.Equal(3).
                CountryTable.Rows[0].Capital.Should.Equal("London").
                Do(x => x.CountryTable.Rows[r => r.Capital == "Paris"], x =>
                {
                    x.Should.Exist();
                    x.Country.Should.Equal("France");
                }).
                CountryTable.Rows["Germany", "Berlin"].Should.Exist();
        }

        [Test]
        public void Table_ByColumnIndex()
        {
            page.
                CountryByColumnIndexTable.Should.Exist().
                CountryByColumnIndexTable.Rows.Count.Should.Equal(3).
                CountryByColumnIndexTable.Rows[0].CapitalName.Should.Equal("London").
                Do(x => x.CountryByColumnIndexTable.Rows[r => r.CapitalName == "Paris"], x =>
                {
                    x.Should.Exist();
                    x.CountryName.Should.Equal("France");
                }).
                CountryByColumnIndexTable.Rows["Germany", "Berlin"].Should.Exist();
        }
    }
}
