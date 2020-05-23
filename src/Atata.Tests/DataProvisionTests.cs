using NUnit.Framework;

namespace Atata.Tests
{
    public class DataProvisionTests : UITestFixture
    {
        [Test]
        public void UrlProviderMultipleQueryParametersTest()
        {
            Go.To<StubPage>(url: "http://localhost/basiccontrols?empty=&id=1&type=a&type=b&date=1&date=2&").
                PageUrl.QueryParameters.Count.ExpectTo.Equal(6).
                PageUrl.QueryParameters["id"].ExpectTo.Equal("1").
                PageUrl.QueryParameters["empty"].ExpectTo.BeEmpty().
                PageUrl.QueryParameters.Get<int>("id").ExpectTo.Equal(1).
                PageUrl.QueryParameters.GetAll("type").ExpectTo.EqualSequence("a", "b").
                PageUrl.QueryParameters.GetAll<int>("date").ExpectTo.EqualSequence(1, 2);
        }

        [Test]
        public void UrlProviderNoParametersTest()
        {
            Go.To<StubPage>(url: "http://example.com").
                PageUrl.QueryParameters.Count.ExpectTo.Equal(0).
                PageUrl.QueryParameters["id"].ExpectTo.BeNull().
                PageUrl.QueryParameters.Get<string>("id").ExpectTo.BeNull().
                PageUrl.QueryParameters.GetAll<string>("id").ExpectTo.HaveCount(0).
                PageUrl.Fragment.ExpectTo.BeEmpty().
                PageUrl.AbsolutePath.ExpectTo.Equal("/");
        }

        [Test]
        public void UrlProviderOtherParametersTest()
        {
            Go.To<StubPage>(url: "http://example.com/index#anchor").
                PageUrl.Fragment.ExpectTo.Equal("#anchor").
                PageUrl.AbsolutePath.ExpectTo.Equal("/index");
        }
    }
}
