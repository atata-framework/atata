namespace Atata
{
    [UIComponent("*", IgnoreNameEndings = "Button,Link")]
    public class Clickable<TNavigateTo, TOwner> : ClickableBase<TNavigateTo, TOwner>
        where TNavigateTo : PageObject<TNavigateTo>, new()
        where TOwner : PageObject<TOwner>
    {
        private bool setAllGeneratables;

        protected override TNavigateTo GetResult()
        {
            TNavigateTo newObject = Owner.GoTo<TNavigateTo>();
            if (setAllGeneratables)
                newObject.SetAllGeneratables();
            return newObject;
        }

        protected internal override void ApplyMetadata(UIComponentMetadata metadata)
        {
            base.ApplyMetadata(metadata);

            setAllGeneratables = metadata.GetFirstOrDefaultPropertyAttribute<SetAllGeneratablesAttribute>() != null;
        }
    }
}
