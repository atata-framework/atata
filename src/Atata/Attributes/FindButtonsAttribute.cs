using System;

namespace Atata
{
    public class FindButtonsAttribute : FindControlsAttribute
    {
        public FindButtonsAttribute(FindTermBy by)
            : base(typeof(Button<>), by)
        {
        }

        public FindButtonsAttribute(Type finderType)
            : base(typeof(Button<>), finderType)
        {
        }
    }
}
