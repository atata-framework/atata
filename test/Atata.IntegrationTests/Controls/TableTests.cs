namespace Atata.IntegrationTests.Controls;

public class TableTests : WebDriverSessionTestSuite
{
    private TablePage _page;

    protected override void OnSetUp() =>
        _page = Go.To<TablePage>();

    [Test]
    public void Headers() =>
        _page
            .SimpleTable.Headers.Should.HaveCount(2)
            .SimpleTable.Headers["First Name"].Content.Should.Be("First Name")
            .SimpleTable.Headers["Last Name"].Content.Should.Be("Last Name")
            .SimpleTable.Headers.Contents.Should.EqualSequence("First Name", "Last Name");

    [Test]
    public void OfTypeWithoutTRow() =>
        _page
            .SimpleTable.Should.BePresent()
            .SimpleTable.Rows.Count.Should.Be(4)
            .SimpleTable.Headers.Should.HaveCount(2)
            .SimpleTable.Headers.Should.Not.HaveCount(3)
            .SimpleTable.Headers.Should.ContainHavingContent(TermMatch.Equals, "First Name", "Last Name")
            .SimpleTable.Rows[0].Content.Should.Contain("John")
            .SimpleTable.Rows[1].Content.Should.Contain("Jane Smith")
            .Do(x => x.SimpleTable.Rows["Jack"], x =>
            {
                x.Should.BePresent();
                x.Content.Should.Contain("Jameson");
            })
            .SimpleTable.Rows.IndexOf(x => x.Content == "Sam Jackson").Should.Be(3)
            .SimpleTable.Rows["Jack", "Jameson"].Should.BePresent()
            .SimpleTable.Rows.Should.ContainHavingContent(TermMatch.Contains, "Jameson")
            .SimpleTable.Rows["Jack Jameson"].Should.Not.BePresent()
            .SimpleTable.Rows.Should.Not.ContainHavingContent(TermMatch.Equals, "Jameson")
            .SimpleTable.Rows.Contents.Should.Contain("John Smith");

    [Test]
    public void OfTypeWithTRow() =>
        _page
            .ComplexTable.Should.BePresent()
            .ComplexTable.Rows.Count.Should.Be(4)
            .ComplexTable.Headers.Should.HaveCount(2)
            .ComplexTable.Headers.Should.ContainHavingContent(TermMatch.Equals, "First Name", "Last Name")
            .ComplexTable.Rows[0].FirstName.Should.Be("John")
            .ComplexTable.Rows[1].FirstName.Should.Be("Jane")
            .Do(x => x.ComplexTable.Rows[r => r.FirstName == "Jack"], x =>
            {
                x.Should.BePresent();
                x.LastName.Should.Be("Jameson");
            })
            .ComplexTable.Rows.IndexOf(r => r.FirstName == "Jack" && r.LastName == "Jameson").Should.Be(2)
            .ComplexTable.Rows.IndexOf(r => r.FirstName == "Unknown").Should.Be(-1)
            .ComplexTable.Rows[r => r.FirstName == "Jack" && r.LastName == "Jameson"].Should.BePresent()
            .ComplexTable.Rows.Should.Contain(r => r.FirstName == "Jack" && r.LastName == "Jameson")
            .ComplexTable.Rows.Should.Not.Contain(r => r.FirstName == "Jason")
            .ComplexTable.Rows[r => r.FirstName == "Jason"].Should.Not.BePresent()
            .ComplexTable.Rows["Jack", "Jameson"].Should.BePresent()
            .ComplexTable.Rows["Jack Jameson"].Should.Not.BePresent()
            .ComplexTable.Rows.SelectData(x => x.FirstName).Should.Contain("John", "Jane", "Jack");

    [Test]
    public void WithFindByIndexAttribute() =>
        _page
            .CountryTable.Should.BePresent()
            .CountryTable.Rows.Count.Should.Be(3)
            .CountryTable.Rows[0].Capital.Should.Be("London")
            .CountryTable.Rows.IndexOf(r => r.Capital.Value.StartsWith("london", StringComparison.CurrentCultureIgnoreCase)).Should.Be(0)
            .CountryTable.Rows.IndexOf(r => r.Capital == "Paris").Should.Be(1)
            .Do(x => x.CountryTable.Rows[r => r.Capital == "Paris"], x =>
            {
                x.Should.BePresent();
                x.Country.Should.Be("France");
            })
            .CountryTable.Rows["Germany", "Berlin"].Should.BePresent();

    [Test]
    public void WithFindByColumnIndexAttributeOnRow() =>
        _page
            .CountryByColumnIndexTable.Should.BePresent()
            .CountryByColumnIndexTable.Rows.Count.Should.Be(3)
            .CountryByColumnIndexTable.Rows[0].CapitalName.Should.Be("London")
            .Do(x => x.CountryByColumnIndexTable.Rows[r => r.CapitalName == "Paris"], x =>
            {
                x.Should.BePresent();
                x.CountryName.Should.Be("France");
            })
            .CountryByColumnIndexTable.Rows["Germany", "Berlin"].Should.BePresent();

    [Test]
    public void WhenEmpty() =>
        _page
            .EmptyTable.Should.BePresent()
            .EmptyTable.Rows.Count.Should.Be(0)
            .EmptyTable.Rows.Should.BeEmpty()
            .EmptyTable.Rows[x => x.Name == "missing"].Should.Not.BePresent();

    [Test]
    public void WhenRowIsAppending() =>
        _page
            .CountryTable.Rows.Count.Should.Be(3)
            .AddUsa.Click()
            .CountryTable.Rows[x => x.Country == "USA"].Capital.Should.Be("Washington")
            .CountryTable.Rows.Count.Should.Be(4);

    [Test]
    public void WhenRowIsAppending_WithDelay() =>
        _page
            .CountryTable.Rows.Count.Should.Be(3)
            .AddChina.Click()
            .CountryTable.Rows[x => x.Country == "China"].Capital.Should.Be("Beijing")
            .ClearChina.Click()

            .CountryTable.Rows.Count.Should.Be(3)
            .AddChina.Click()
            .CountryTable.Rows[3].Capital.Click()
            .CountryTable.Rows.Count.Should.Be(4)
            .ClearChina.Click()

            .CountryTable.Rows.Count.Should.Be(3)
            .AddChina.Click()
            .CountryTable.Rows.Count.Should.Be(4)
            .ClearChina.Click()

            .CountryTable.Rows.Count.Should.Be(3)
            .AddChina.Click()
            .CountryTable.Rows.IndexOf(x => x.Capital == "Beijing").Should.Be(3)
            .ClearChina.Click()

            .CountryTable.Rows.Count.Should.Be(3)
            .AddChina.Click()
            .CountryTable.Rows.Count.Should.AtOnce.Be(3)
            .CountryTable.Rows.SelectData(x => x.Capital).Should.Contain("Beijing");

    [Test]
    public void Rows_GetByXPathCondition() =>
        _page.CountryTable.Rows.GetByXPathCondition("Paris", @"td[2][.='Paris']").Country
            .Should.Be("France");

    [Test]
    public void WhenInsideAnotherTable()
    {
        var sut = _page.InsideAnotherTable;

        sut.Should.BePresent();
        sut.Rows.Count.Should.Be(1);
        sut.Rows[0].Key.Should.Be("A");
        sut.Rows[0].Value.Should.Be(1);
        sut.Rows[x => x.Key == "A"].Value.Should.Be(1);
    }

    [Test]
    public void Rows_ForEach()
    {
        Queue<string> expectedClountryNames = new(
        [
            "England",
            "France",
            "Germany"
        ]);

        _page.CountryTable.Rows.ForEach(
            x => x.Country.Value.Should().Be(expectedClountryNames.Dequeue()));
        Assert.That(expectedClountryNames, Is.Empty);
    }

    [Test]
    public void Rows_ForEach_WithCustomControlDefinitionOfRow()
    {
        Queue<string> expectedClountryNames = new(
        [
            "France",
            "Germany"
        ]);

        _page.CountryTableWithCustomControlDefinitionOfRow.Rows.ForEach(
            x => x.Country.Value.Should().Be(expectedClountryNames.Dequeue()));
        Assert.That(expectedClountryNames, Is.Empty);
    }
}
