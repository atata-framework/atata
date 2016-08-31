using OpenQA.Selenium;

namespace Atata
{
    public interface IScreenshotConsumer
    {
        void Take(Screenshot screenshot, int number, string title);
    }
}
