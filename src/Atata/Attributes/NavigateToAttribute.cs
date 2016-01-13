using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Class)]
    public class NavigateToAttribute : Attribute
    {
        public NavigateToAttribute(string url)
        {
            Url = url;
        }

        public string Url { get; private set; }
    }
}
