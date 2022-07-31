using System;
using NUnit.Framework;

namespace Atata.IntegrationTests
{
    public class CustomControlTests : UITestFixture
    {
        [Test]
        public void Control_Custom_DatePicker_WithFormat()
        {
            var control = Go.To<InputPage>().DatePicker;

            control.Should.BeNull();

            DateTime value = new(2018, 7, 11);
            control.Set(value);
            control.Should.Equal(value);

            control.Set(null);
            control.Should.BeNull();
        }
    }
}
