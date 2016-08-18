using NUnit.Framework;

namespace Atata.Tests
{
    public class TableTest : AutoTest
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
                SimpleTable.Headers.Should.HaveCount(2).
                SimpleTable.Headers.Should.Not.HaveCount(3).
                SimpleTable.Headers.Should.ContainHavingContent(TermMatch.Equals, "First Name", "Last Name").
                SimpleTable.Rows[0].Content.Should.Contain("John").
                SimpleTable.Rows[1].Content.Should.Contain("Jane Smith").
                Do(x => x.SimpleTable.Rows["Jack"], x =>
                {
                    x.Should.Exist();
                    x.Content.Should.Contain("Jameson");
                }).
                SimpleTable.Rows["Jack", "Jameson"].Should.Exist().
                SimpleTable.Rows.Should.ContainHavingContent(TermMatch.Contains, "Jameson").
                SimpleTable.Rows["Jack Jameson"].Should.Not.Exist().
                SimpleTable.Rows.Should.Not.ContainHavingContent(TermMatch.Equals, "Jameson");
        }

        [Test]
        public void Table_Complex()
        {
            page.
                ComplexTable.Should.Exist().
                ComplexTable.Rows.Count.Should.Equal(4).
                ComplexTable.Headers.Should.HaveCount(2).
                ComplexTable.Headers.Should.ContainHavingContent(TermMatch.Equals, "First Name", "Last Name").
                ComplexTable.Rows[0].FirstName.Should.Equal("John").
                ComplexTable.Rows[1].FirstName.Should.Equal("Jane").
                Do(x => x.ComplexTable.Rows[r => r.FirstName == "Jack"], x =>
                {
                    x.Should.Exist();
                    x.LastName.Should.Equal("Jameson");
                }).
                ComplexTable.Rows[r => r.FirstName == "Jack" && r.LastName == "Jameson"].Should.Exist().
                ComplexTable.Rows.Should.Contain(r => r.FirstName == "Jack" && r.LastName == "Jameson").
                ComplexTable.Rows.Should.Not.Contain(r => r.FirstName == "Jason").
                ComplexTable.Rows[r => r.FirstName == "Jason"].Should.Not.Exist().
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
