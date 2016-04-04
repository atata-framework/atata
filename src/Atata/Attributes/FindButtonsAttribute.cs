using System;

namespace Atata
{
    public class FindButtonsAttribute : FindControlsAttribute
    {
        public FindButtonsAttribute(FindTermBy by)
            : base(typeof(ButtonControl<>), by)
        {
        }

        public FindButtonsAttribute(Type finderType)
            : base(typeof(ButtonControl<>), finderType)
        {
        }
    }
}
