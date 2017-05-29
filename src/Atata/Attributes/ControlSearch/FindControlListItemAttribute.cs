using System;

namespace Atata
{
    internal class FindControlListItemAttribute : FindAttribute
    {
        protected override Type DefaultStrategy
        {
            get { return null; }
        }
    }
}
