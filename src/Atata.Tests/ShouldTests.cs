using NUnit.Framework;

namespace Atata.Tests
{
    public class ShouldTests : UITestFixture
    {
        private const string Country1Name = "England";
        private const string Country2Name = "France";
        private const string Country3Name = "Germany";
        private const string MissingCountryName = "Missing";

        [Test]
        public void Should_BeEquivalent()
        {
            var should = Go.To<TablePage>().
                CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

            should.BeEquivalent(Country1Name, Country2Name, Country3Name);
            should.BeEquivalent(Country2Name, Country1Name, Country3Name);

            Assert.Throws<AssertionException>(() =>
                should.BeEquivalent(Country1Name, Country2Name));

            should.Not.BeEquivalent(Country1Name, Country2Name, Country3Name, MissingCountryName);

            Assert.Throws<AssertionException>(() =>
                should.Not.BeEquivalent(Country3Name, Country1Name, Country2Name));
        }

        [Test]
        public void Should_EqualSequence()
        {
            var should = Go.To<TablePage>().
                CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

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
        public void Should_Contain()
        {
            var should = Go.To<TablePage>().
                CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

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
        public void Should_ContainHavingContent()
        {
            var should = Go.To<TablePage>().
                CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

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
        public void Should_Contain_TermMatch()
        {
            var should = Go.To<TablePage>().
                CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

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
        public void Should_Contain_Predicate()
        {
            var should = Go.To<TablePage>().
                CountryTable.Rows.SelectData(x => x.Country).Should.AtOnce;

            should.Contain(x => x == Country1Name);
            should.Contain(x => x.Value == MissingCountryName || x.Value == Country3Name);

            Assert.Throws<AssertionException>(() =>
                should.Contain(x => x == MissingCountryName));

            should.Not.Contain(x => x == MissingCountryName);

            Assert.Throws<AssertionException>(() =>
                should.Not.Contain(x => x == Country1Name));
        }
    }
}
