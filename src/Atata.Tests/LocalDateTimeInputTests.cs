using System;
using NUnit.Framework;

namespace Atata.Tests
{
    public class LocalDateTimeInputTests : UITestFixture
    {
        private const string ControlValueFormat = "yyyy-MM-ddTHH:mm";

        private LocalDateTimeInputPage page;

        protected override void OnSetUp()
        {
            page = Go.To<LocalDateTimeInputPage>();
        }

        [Test]
        public void LocalDateTimeInput()
        {
            var control = page.Regular;
            var outputControl = page.RegularOutput;

            control.Should.BeNull();

            var testValue = new DateTime(2019, 11, 27, 20, 45, 0);

            control.Set(testValue);
            control.Should.Equal(testValue);
            outputControl.Should.Equal(testValue);

            control.Clear();
            control.Should.BeNull();
            outputControl.Should.BeNull();

            control.Append("invalid");
            control.Should.BeNull();
            outputControl.Should.BeNull();

            testValue = new DateTime(2011, 1, 2, 8, 00, 0);
            control.Set(testValue);
            control.Should.Equal(testValue);
            outputControl.Should.Equal(testValue);
        }
    }
}
