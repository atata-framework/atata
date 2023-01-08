using System;
using OpenQA.Selenium;

namespace Atata
{
    internal class GoOptions
    {
        public string Url { get; set; }

        public Func<string> WindowNameResolver { get; set; }

        public WindowType? NewWindowType { get; set; }

        public bool Navigate { get; set; }

        public bool Temporarily { get; set; }

        public string NavigationTarget { get; set; }
    }
}
