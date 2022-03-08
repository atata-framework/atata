using System;
using FluentAssertions;
using NUnit.Framework;

namespace Atata.Tests
{
    public class MessageBoxTests : UITestFixture
    {
        protected override bool ReuseDriver => false;

        [Test]
        public void CloseAlertBox()
        {
            Go.To<MessageBoxPage>()
                .AlertButton.Click()
                .PageTitle.Should.StartWith("Message Box");
        }

        [Test]
        public void WaitForAlertBox()
        {
            Go.To<MessageBoxPage>().AlertWithDelayButton.Click();

            AtataContext.Current.Driver.SwitchTo().Alert().Text.Should().Be("Alert with delay!!!");
        }

        [Test]
        public void WaitForAlertBox_Timeout()
        {
            var sut = Go.To<MessageBoxPage>().NoneButton;
            sut.Metadata.Push(new WaitForAlertBoxAttribute { Timeout = 1 });

            Assert.Throws<TimeoutException>(() =>
                sut.Click());
        }

        [Test]
        public void CloseConfirmBox_Accept()
        {
            Go.To<MessageBoxPage>()
                .ConfirmButton.Click()
                .PageTitle.Should.StartWith("Go");
        }

        [Test]
        public void CloseConfirmBox_Reject()
        {
            Go.To<MessageBoxPage>()
                .ConfirmButtonWithReject.Click()
                .PageTitle.Should.StartWith("Message Box");
        }
    }
}
