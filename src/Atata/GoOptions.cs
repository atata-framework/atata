using OpenQA.Selenium;

namespace Atata
{
    internal class GoOptions
    {
        public string Url { get; set; }

        public string WindowName { get; set; }

        public WindowType? NewWindowType { get; set; }

        public bool Navigate { get; set; }

        public bool Temporarily { get; set; }
    }
}
