namespace Atata
{
    /// <summary>
    /// Represents the behavior to find an item of <see cref="OptionList{TValue, TOwner}"/> control by parent element content.
    /// </summary>
    public class FindItemByParentContentAttribute : FindItemByRelativeElementContentAttribute
    {
        public const string ParentElementXPath = "parent::*";

        public FindItemByParentContentAttribute()
            : base(ParentElementXPath)
        {
        }

        public FindItemByParentContentAttribute(TermCase termCase)
            : base(ParentElementXPath, termCase)
        {
        }

        public FindItemByParentContentAttribute(TermMatch match)
            : base(ParentElementXPath, match)
        {
        }

        public FindItemByParentContentAttribute(TermMatch match, TermCase termCase)
            : base(ParentElementXPath, match, termCase)
        {
        }
    }
}
