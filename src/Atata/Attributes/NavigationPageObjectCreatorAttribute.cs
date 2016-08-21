using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NavigationPageObjectCreatorAttribute : Attribute
    {
        public NavigationPageObjectCreatorAttribute(Func<object> creator)
        {
            Creator = creator;
        }

        public Func<object> Creator { get; private set; }
    }
}
