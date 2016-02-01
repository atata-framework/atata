using System;

namespace Atata
{
    public class FindLinksAttribute : FindControlsAttribute
    {
        public FindLinksAttribute(FindTermBy by)
            : base(typeof(Button<>), by)
        {
        }

        public FindLinksAttribute(Type finderType)
            : base(typeof(Button<>), finderType)
        {
        }
    }
}
