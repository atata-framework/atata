using System;

namespace Atata
{
    public class FindLinksAttribute : FindControlsAttribute
    {
        public FindLinksAttribute(FindTermBy by)
            : base(typeof(Link<>), by)
        {
        }

        public FindLinksAttribute(Type finderType)
            : base(typeof(Link<>), finderType)
        {
        }
    }
}
