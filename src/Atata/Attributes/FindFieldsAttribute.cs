using System;

namespace Atata
{
    public class FindFieldsAttribute : FindControlsAttribute
    {
        public FindFieldsAttribute(FindTermBy by)
            : base(typeof(Field<,>), by)
        {
        }

        public FindFieldsAttribute(Type finderType)
            : base(typeof(Field<,>), finderType)
        {
        }
    }
}
