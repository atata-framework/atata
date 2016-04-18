using System;

namespace Atata
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GoTemporarilyAttribute : Attribute
    {
        public GoTemporarilyAttribute(bool isTemporarily = true)
        {
            IsTemporarily = isTemporarily;
        }

        public bool IsTemporarily { get; private set; }
    }
}
