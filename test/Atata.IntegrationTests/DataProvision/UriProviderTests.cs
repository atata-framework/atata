﻿#nullable enable

namespace Atata.IntegrationTests.DataProvision;

public sealed class UriProviderTests : WebDriverSessionTestSuite
{
    [Test]
    public void InComplex() =>
        Go.To<OrdinaryPage>(url: "/index?id=15&type=a&type=b&num=1&num=2#some-fragment")
            .AggregateAssert(x => x
                .PageUri.Path.Should.Equal("/index")

                .PageUri.Query.Should.Be("?id=15&type=a&type=b&num=1&num=2")

                .PageUri.Query.Parameters.Count.Should.Equal(5)

                .PageUri.Query.Parameters["id"].Should.Equal("15")
                .PageUri.Query.Parameters.Get<int>("id").Should.Equal(15)

                .PageUri.Query.Parameters.GetAll("type").Should.EqualSequence("a", "b")
                .PageUri.Query.Parameters.GetAll<int>("num").Should.EqualSequence(1, 2)

                .PageUri.Fragment.Should.Equal("#some-fragment"));

    [Test]
    public void Query_Parameters() =>
        Go.To<OrdinaryPage>(url: "/index?empty1=&empty2&empty2=&id=1&type=a&type=b&date=1&date=2&")
            .AggregateAssert(x => x
                .PageUri.Query.Parameters.Count.Should.Equal(8)
                .PageUri.Query.Parameters["id"].Should.Equal("1")
                .PageUri.Query.Parameters["empty1"].Should.BeEmpty()
                .PageUri.Query.Parameters["empty2"].Should.BeEmpty()
                .PageUri.Query.Parameters.GetAll("empty1").Should.BeEquivalent(string.Empty)
                .PageUri.Query.Parameters.GetAll("empty2").Should.BeEquivalent(string.Empty, string.Empty)
                .PageUri.Query.Parameters.GetAll<Guid?>("empty1").Should.BeEquivalent(null as Guid?)
                .PageUri.Query.Parameters.Get<int?>("empty1").Should.BeNull()
                .PageUri.Query.Parameters.Get<int>("id").Should.Equal(1)
                .PageUri.Query.Parameters.GetAll("type").Should.EqualSequence("a", "b")
                .PageUri.Query.Parameters.GetAll<int>("date").Should.EqualSequence(1, 2)
                .PageUri.Query.Parameters.Get<int>("empty1").Should.Throw<ArgumentException>());

    [Test]
    public void Query_Parameters_WhenEmpty() =>
        Go.To<OrdinaryPage>(url: "/")
            .AggregateAssert(x => x
                .PageUri.Query.Parameters.Count.Should.Equal(0)
                .PageUri.Query.Parameters["id"].Should.BeNull()
                .PageUri.Query.Parameters.Get<string>("id").Should.BeNull()
                .PageUri.Query.Parameters.Get<int?>("id").Should.BeNull()
                .PageUri.Query.Parameters.GetAll<string>("id").Should.BeEmpty()
                .PageUri.Fragment.Should.BeEmpty()
                .PageUri.Path.Should.Equal("/"));

    [Test]
    public void Fragment() =>
        Go.To<OrdinaryPage>(url: "/index?q=1#anchor")
            .PageUri.Fragment.Should.Equal("#anchor");

    [Test]
    public void Path() =>
        Go.To<OrdinaryPage>(url: "/index?q=1#anchor")
            .PageUri.Path.Should.Equal("/index");

    [Test]
    public void PathAndQuery() =>
        Go.To<OrdinaryPage>(url: "/index?q=1#anchor")
            .PageUri.PathAndQuery.Should.Equal("/index?q=1");

    [Test]
    public void Relative() =>
        Go.To<OrdinaryPage>(url: $"/index?q={Uri.EscapeDataString("?")}#anchor")
            .PageUri.Relative.Should.Equal("/index?q=%3F#anchor");

    [Test]
    public void RelativeUnescaped() =>
        Go.To<OrdinaryPage>(url: $"/index?q={Uri.EscapeDataString("?")}#anchor")
            .PageUri.RelativeUnescaped.Should.Equal("/index?q=?#anchor");

    [Test]
    public void Absolute() =>
        Go.To<OrdinaryPage>(url: $"/index?q={Uri.EscapeDataString("?")}#anchor")
            .PageUri.Absolute.Should.Equal(BaseUrl + "/index?q=%3F#anchor");

    [Test]
    public void AbsoluteUnescaped() =>
        Go.To<OrdinaryPage>(url: $"/index?q={Uri.EscapeDataString("?")}#anchor")
            .PageUri.AbsoluteUnescaped.Should.Equal(BaseUrl + "/index?q=?#anchor");

    [Test]
    public void GetComponents_UriEscaped() =>
        Go.To<OrdinaryPage>(url: $"/index?q=%3F#anchor")
            .PageUri.GetComponents(UriComponents.Query | UriComponents.Fragment).Should.Equal("?q=%3F#anchor");

    [Test]
    public void GetComponents_Unescaped() =>
        Go.To<OrdinaryPage>(url: $"/index?q=%3F#anchor")
            .PageUri.GetComponents(UriComponents.Query, UriFormat.Unescaped).Should.Equal("q=?");
}
