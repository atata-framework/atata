﻿namespace Atata.IntegrationTests.Verification;

public class ShouldTests : WebDriverSessionTestSuite
{
    private const string Country1Name = "England";

    private const string Country2Name = "France";

    private const string Country3Name = "Germany";

    private const string MissingCountryName = "Missing";

    [Test]
    public void BeEquivalent()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.BeEquivalent(Country1Name, Country2Name, Country3Name);
        should.BeEquivalent(Country2Name, Country1Name, Country3Name);

        Assert.Throws<AssertionException>(() =>
            should.BeEquivalent(Country1Name, Country2Name));

        should.Not.BeEquivalent(Country1Name, Country2Name, Country3Name, MissingCountryName);

        Assert.Throws<AssertionException>(() =>
            should.Not.BeEquivalent(Country3Name, Country1Name, Country2Name));
    }

    [Test]
    public void BeEquivalent_Empty() =>
        Go.To<TablePage>()
           .FindAll<H6<TablePage>>().Should.BeEquivalent(new string[0]);

    [Test]
    public void EqualSequence()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.EqualSequence(Country1Name, Country2Name, Country3Name);

        Assert.Throws<AssertionException>(() =>
            should.EqualSequence(Country1Name, Country3Name, Country2Name));

        Assert.Throws<AssertionException>(() =>
            should.EqualSequence(Country1Name));

        should.Not.EqualSequence(Country1Name, Country2Name, Country3Name, MissingCountryName);
        should.Not.EqualSequence(Country3Name, Country1Name, Country2Name);
        should.Not.EqualSequence(Country1Name);

        Assert.Throws<AssertionException>(() =>
            should.Not.EqualSequence(Country1Name, Country2Name, Country3Name));
    }

    [Test]
    public void EqualSequence_Empty() =>
        Go.To<TablePage>()
           .FindAll<H6<TablePage>>().Should.EqualSequence(new string[0]);

    [Test]
    public void ContainSingle_FromOne()
    {
        var should = Go.To<TablePage>()
            .SingleItemTable.Rows.SelectData(x => x.Key).Should.AtOnce;

        should.ContainSingle();

        Assert.Throws<AssertionException>(() =>
            should.Not.ContainSingle());
    }

    [Test]
    public void ContainSingle_FromMany()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.Not.ContainSingle();

        Assert.Throws<AssertionException>(() =>
            should.ContainSingle());
    }

    [Test]
    public void ContainSingle_Equal_FromOne()
    {
        var should = Go.To<TablePage>()
            .SingleItemTable.Rows.SelectData(x => x.Key).Should.AtOnce;

        should.ContainSingle("Some item");

        Assert.Throws<AssertionException>(() =>
            should.ContainSingle("Another item"));

        should.Not.ContainSingle("Another item");

        Assert.Throws<AssertionException>(() =>
            should.Not.ContainSingle("Some item"));
    }

    [Test]
    public void ContainSingle_Equal_FromMany_Single()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.ContainSingle(Country1Name);

        Assert.Throws<AssertionException>(() =>
            should.ContainSingle(MissingCountryName));

        should.Not.ContainSingle(MissingCountryName);
    }

    [Test]
    public void ContainSingle_Equal_FromMany_Duplicate()
    {
        var should = Go.To<TablePage>()
            .DuplicateItemsTable.Rows.SelectData(x => x.Key).Should.AtOnce;

        should.Not.ContainSingle("Some item");

        Assert.Throws<AssertionException>(() =>
            should.ContainSingle("Some item"));
    }

    [Test]
    public void ContainSingle_Predicate_FromOne()
    {
        var should = Go.To<TablePage>()
            .SingleItemTable.Rows.Should.AtOnce;

        should.ContainSingle(x => x.Key == "Some item");

        Assert.Throws<AssertionException>(() =>
            should.ContainSingle(x => x.Key == "Another item"));

        should.Not.ContainSingle(x => x.Key == "Another item");

        Assert.Throws<AssertionException>(() =>
            should.Not.ContainSingle(x => x.Key == "Some item"));
    }

    [Test]
    public void ContainSingle_Predicate_FromMany_Single()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.Should.AtOnce;

        should.ContainSingle(x => x.Country == Country1Name);

        Assert.Throws<AssertionException>(() =>
            should.ContainSingle(x => x.Country == MissingCountryName));

        should.Not.ContainSingle(x => x.Country == MissingCountryName);
    }

    [Test]
    public void ContainSingle_Predicate_FromMany_Duplicate()
    {
        var should = Go.To<TablePage>()
            .DuplicateItemsTable.Rows.Should.AtOnce;

        should.Not.ContainSingle(x => x.Key == "Some item");

        Assert.Throws<AssertionException>(() =>
            should.ContainSingle(x => x.Key == "Some item"));
    }

    [Test]
    public void ContainExactly_Item()
    {
        var should = Go.To<TablePage>()
            .DuplicateItemsTable.Rows.SelectData(x => x.Key).Should.AtOnce;

        should.ContainExactly(2, "Some item");

        Assert.Throws<AssertionException>(() =>
            should.ContainExactly(2, "Missing"));

        should.Not.ContainExactly(3, "Some item");
        should.Not.ContainExactly(3, "Missing");
    }

    [Test]
    public void ContainExactly_Predicate()
    {
        var should = Go.To<TablePage>()
            .DuplicateItemsTable.Rows.Should.AtOnce;

        should.ContainExactly(2, x => x.Key == "Some item");

        Assert.Throws<AssertionException>(() =>
            should.ContainExactly(2, x => x.Key == "Missing"));

        should.Not.ContainExactly(3, x => x.Key == "Some item");
        should.Not.ContainExactly(3, x => x.Key == "Missing");
    }

    [Test]
    public void Contain()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.Contain(Country1Name, Country2Name, Country3Name);
        should.Contain(Country2Name, Country1Name);

        Assert.Throws<AssertionException>(() =>
            should.Contain(Country1Name, MissingCountryName));

        should.Not.Contain(MissingCountryName);

        Assert.Throws<AssertionException>(() =>
            should.Not.Contain(Country1Name, MissingCountryName));
        Assert.Throws<AssertionException>(() =>
            should.Not.Contain(Country3Name, Country1Name, Country2Name));
    }

    [Test]
    public void ContainHavingContent()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.ContainHavingContent(TermMatch.Equals, Country1Name, Country2Name, Country3Name);
        should.ContainHavingContent(TermMatch.StartsWith, Country2Name, Country1Name);
        should.ContainHavingContent(TermMatch.Contains, "a", "e");

        Assert.Throws<AssertionException>(() =>
            should.ContainHavingContent(TermMatch.Equals, Country1Name, MissingCountryName));
        Assert.Throws<AssertionException>(() =>
            should.ContainHavingContent(TermMatch.Contains, "a", "v"));

        should.Not.ContainHavingContent(TermMatch.Contains, MissingCountryName);
        should.Not.ContainHavingContent(TermMatch.Contains, "v", "w");

        Assert.Throws<AssertionException>(() =>
            should.Not.ContainHavingContent(TermMatch.EndsWith, Country1Name, MissingCountryName));
        Assert.Throws<AssertionException>(() =>
            should.Not.ContainHavingContent(TermMatch.StartsWith, Country3Name, Country1Name, Country2Name));
        Assert.Throws<AssertionException>(() =>
            should.Not.ContainHavingContent(TermMatch.Contains, "a", "v"));
    }

    [Test]
    public void Contain_TermMatch()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.Contain(TermMatch.Equals, Country1Name, Country2Name, Country3Name);
        should.Contain(TermMatch.StartsWith, Country2Name, Country1Name);
        should.Contain(TermMatch.Contains, "a", "e");

        Assert.Throws<AssertionException>(() =>
            should.Contain(TermMatch.Equals, Country1Name, MissingCountryName));
        Assert.Throws<AssertionException>(() =>
            should.Contain(TermMatch.Contains, "a", "v"));

        should.Not.Contain(TermMatch.Contains, MissingCountryName);
        should.Not.Contain(TermMatch.Contains, "v", "w");

        Assert.Throws<AssertionException>(() =>
            should.Not.Contain(TermMatch.EndsWith, Country1Name, MissingCountryName));
        Assert.Throws<AssertionException>(() =>
            should.Not.Contain(TermMatch.StartsWith, Country3Name, Country1Name, Country2Name));
        Assert.Throws<AssertionException>(() =>
            should.Not.Contain(TermMatch.Contains, "a", "v"));
    }

    [Test]
    public void Contain_Predicate()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

        should.Contain(x => x == Country1Name);
        should.Contain(x => x.Value == MissingCountryName || x.Value == Country3Name);

        Assert.Throws<AssertionException>(() =>
            should.Contain(x => x == MissingCountryName));

        should.Not.Contain(x => x == MissingCountryName);

        Assert.Throws<AssertionException>(() =>
            should.Not.Contain(x => x == Country1Name));
    }

    [Test]
    public void Equal_Delayed() =>
        Go.To<WaitingPage>()
            .WaitAndUpdateValue.Click()
            .ValueBlock.Should.Be("New value");

    [Test]
    public void Equal_Delayed_WithParentReset() =>
        Go.To<WaitingPage>()
            .WaitAndUpdateValue.Click()
            .ValueContainer.ValueBlock.Should.Be("New value");

    [Test]
    public void MatchRegex()
    {
        var should = Go.To<ContentPage>()
            .NumberAsText.Should.AtOnce;

        should.MatchRegex(@"^\d{3}.\d{2}$");

        Assert.Throws<AssertionException>(() =>
            should.Not.MatchRegex(@"^\d{3}.\d{2}$"));

        Assert.Throws<AssertionException>(() =>
            should.MatchRegex(@"^\d{4}.\d{2}$"));
    }

    [Test]
    public void BeFocused()
    {
        var sut = Go.To<InputPage>()
            .TelInput;

        sut.Should.Not.BeFocused();

        sut.Focus();
        sut.Should.BeFocused();
    }

    [Test]
    public void HaveClass()
    {
        var should = Go.To<TablePage>()
            .CountryTable.Should.AtOnce;

        should.HaveClass("table");

        Assert.Throws<AssertionException>(() =>
            should.HaveClass("missing"));

        should.Not.HaveClass("missing");

        Assert.Throws<AssertionException>(() =>
            should.Not.HaveClass("table"));
    }

    [Test]
    public void HaveLength()
    {
        var should = Go.To<WaitingPage>()
            .WaitAndUpdateValue.Click()
            .ValueBlock.Should;

        should.HaveLength(9);
        should.Not.HaveLength(8);

        Assert.Throws<AssertionException>(() => should.HaveLength(10));
    }

    [Test]
    public void BeInAscendingOrder_Item()
    {
        var should = Go.To<TablePage>()
            .OrderedTable.Rows.SelectContentsByExtraXPath("td[1]").Should.AtOnce;

        should.BeInAscendingOrder();

        Assert.Throws<AssertionException>(
            () => should.Not.BeInAscendingOrder());
    }

    [Test]
    public void BeInAscendingOrder_DataProvider()
    {
        var should = Go.To<TablePage>()
            .OrderedTable.Rows.SelectData(x => x.Letter).Should.AtOnce;

        should.BeInAscendingOrder();

        Assert.Throws<AssertionException>(
            () => should.Not.BeInAscendingOrder());
    }

    [Test]
    public void BeInDescendingOrder_Item()
    {
        var should = Go.To<TablePage>()
            .OrderedTable.Rows.SelectContentsByExtraXPath<int>("td[2]").Should.AtOnce;

        should.BeInDescendingOrder();

        Assert.Throws<AssertionException>(
            () => should.Not.BeInDescendingOrder());
    }

    [Test]
    public void BeInDescendingOrder_NullableItem()
    {
        var should = Go.To<TablePage>()
            .OrderedTable.Rows.SelectContentsByExtraXPath<int?>("td[2]").Should.AtOnce;

        should.BeInDescendingOrder();

        Assert.Throws<AssertionException>(
            () => should.Not.BeInDescendingOrder());
    }

    [Test]
    public void BeInDescendingOrder_DataProvider()
    {
        var should = Go.To<TablePage>()
            .OrderedTable.Rows.SelectData(x => x.Number).Should.AtOnce;

        should.BeInDescendingOrder();

        Assert.Throws<AssertionException>(
            () => should.Not.BeInDescendingOrder());
    }
}
