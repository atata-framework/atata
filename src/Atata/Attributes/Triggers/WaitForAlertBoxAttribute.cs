using System;
using OpenQA.Selenium;

namespace Atata
{
    /// <summary>
    /// Indicates to wait for an alert box to be present on the specified event.
    /// Be default occurs after the click.
    /// </summary>
    public class WaitForAlertBoxAttribute : WaitingTriggerAttribute
    {
        public WaitForAlertBoxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context) =>
            context.Log.ExecuteSection(
                new LogSection("Wait for alert box"),
                () =>
                {
                    bool isCompleted = context.Driver
                        .Try(TimeSpan.FromSeconds(Timeout), TimeSpan.FromSeconds(RetryInterval))
                        .Until(driver =>
                        {
                            try
                            {
                                driver.SwitchTo().Alert();
                                return true;
                            }
                            catch (NoAlertPresentException)
                            {
                                return false;
                            }
                        });

                    if (!isCompleted)
                        throw new TimeoutException("Timed out waiting for alert box.");
                });
    }
}
