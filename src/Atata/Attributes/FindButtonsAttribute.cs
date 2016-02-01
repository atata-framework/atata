using System;

namespace Atata
{
    public class FindButtonsAttribute : FindControlsAttribute
    {
        public FindButtonsAttribute(FindTermBy by)
            : base(typeof(Link<>), by)
        {
        }

        public FindButtonsAttribute(Type finderType)
            : base(typeof(Link<>), finderType)
        {
        }
    }
}
