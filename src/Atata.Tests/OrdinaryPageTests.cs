using System.Linq;
using NUnit.Framework;

namespace Atata.Tests
{
    public class OrdinaryPageTests : UITestFixture
    {
        [Test]
        public void OrdinaryPage()
        {
            var page = Go.To<OrdinaryPage>(url: "Input.html");

            Assert.That(LogEntries.Last().Message, Is.EqualTo("Go to \"<ordinary>\" page"));

            page.PageTitle.Should.StartWith("Input");
        }

        [Test]
        public void OrdinaryPage_WithComponentName()
        {
            Go.To(new OrdinaryPage("Custom name"), url: "Input.html");

            Assert.That(LogEntries.Last().Message, Is.EqualTo("Go to \"Custom name\" page"));
        }
    }
}
