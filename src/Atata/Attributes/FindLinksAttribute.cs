using System;

namespace Atata
{
    public class FindLinksAttribute : FindControlsAttribute
    {
        public FindLinksAttribute(FindTermBy by)
            : base(typeof(LinkControl<>), by)
        {
        }

        public FindLinksAttribute(Type finderType)
            : base(typeof(LinkControl<>), finderType)
        {
        }
    }
}
